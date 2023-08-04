

using ERPSystem.BLL.DTO.Auth;
using ERPSystem.DataAccess.Entities.Auth;
using ERPSystem.DataAccess.Entities.UserEntities;
using ERPSystem.DataAccess.Repositories.Interfaces;
using ERPSystem.DataAccess.Repositories.Interfaces.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ERPSystem.BLL.Services.AuthService
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IConfiguration _config;
        private readonly UserManager<User> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        public AuthenticationService(
            IConfiguration config,
            UserManager<User> userManager,
            IUnitOfWork unitOfWork)
        {
            _config = config;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _refreshTokenRepository = _unitOfWork.GetRepository<RefreshToken, IRefreshTokenRepository>(hasCustomRepository: true);
        }
        public async Task<AuthResultDTO> GenerateJWTTokenAsync(User user, RefreshToken rToken)
        {
            var authClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            //var userRoles = await _userManager.GetRolesAsync(user);
            //foreach (var role in userRoles)
            //{
            //    authClaims.Add(new Claim(ClaimTypes.Role, role));
            //}
            var userClaims = await _userManager.GetClaimsAsync(user);
            authClaims.AddRange(userClaims);

            var authSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_config["JWT:Secret"]));
            var token = new JwtSecurityToken(issuer: _config["JWT:Issuer"],
                audience: _config["JWT:Audience"],
                expires: DateTime.UtcNow.AddMinutes(5),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256));
            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
            if (rToken != null)
            {
                var rTokenResponse = new AuthResultDTO()
                {
                    Token = jwtToken,
                    RefreshToken = rToken.RefrToken,
                    ExpiresAt = token.ValidTo
                };
                return rTokenResponse;
            }
            var refreshToken = new RefreshToken()
            {
                JwtId = token.Id,
                IsRevoked = false,
                IdentityUserId = user.Id,
                DateAdded = DateTime.UtcNow,
                DateExpired = DateTime.UtcNow.AddMonths(6),
                RefrToken = Guid.NewGuid().ToString() + "-" + Guid.NewGuid().ToString(),
            };
            await _refreshTokenRepository.CreateAsync(refreshToken);
            await _unitOfWork.SaveChangesAsync();
            var result = new AuthResultDTO()
            {
                Token = jwtToken,
                RefreshToken = refreshToken.RefrToken,
                ExpiresAt = token.ValidTo
            };
            return result;
        }
    }
}
