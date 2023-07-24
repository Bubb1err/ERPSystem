using ERPSystem.BLL.DTO.Auth;
using ERPSystem.DataAccess;
using ERPSystem.DataAccess.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ERPSystem.Web.Controllers
{
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _config;
        private readonly AppDbContext _context;
        private readonly TokenValidationParameters _tokenValidationParameters;
        public AuthController(UserManager<IdentityUser> userManager,
            IConfiguration config, 
            AppDbContext context, 
            TokenValidationParameters tokenValidationParameters)
        {
            _userManager = userManager;
            _config = config;
            _context = context;
            _tokenValidationParameters = tokenValidationParameters;

        }

        [HttpPost("signup")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromForm]RegisterDTO registerDto)
        {
            if(registerDto.Password != registerDto.ConfirmPassword) return BadRequest("Passwords mismatch.");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(registerDto.Email);
            if (user != null)
            {
                return Conflict("User already exists.");
            }
            var newUser = new IdentityUser{ UserName = registerDto.Email, Email = registerDto.Email };
            var result = await _userManager.CreateAsync(newUser, registerDto.Password);

            if (result.Succeeded)
            {
                return Ok("User created successfully.");
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromForm]LoginDTO loginDto)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            var userExist = await _userManager.FindByEmailAsync(loginDto.Email);
            if (userExist != null && await _userManager.CheckPasswordAsync(userExist, loginDto.Password))
            {
                var token = await GenerateJWTTokenAsync(userExist, null);
                return Ok(token);
            }
            if (userExist == null) return RedirectToAction(nameof(Register));
            if (!(await _userManager.CheckPasswordAsync(userExist, loginDto.Password)))
            {
                return BadRequest("Incorrect password.");
            }
            return Unauthorized();
        }
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(string refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken)) return BadRequest("Provide a valid refresh token.");
            var storedToken = await _context.RefreshTokens.FirstOrDefaultAsync(x => x.RefrToken == refreshToken);
            if (storedToken == null) return Unauthorized();
            var dbUser = await _userManager.FindByIdAsync(storedToken.UserId);
            try
            {
                return Ok(await GenerateJWTTokenAsync(dbUser, storedToken));
            }
            catch (SecurityTokenExpiredException)
            {
                if (storedToken.DateExpired >= DateTime.UtcNow)
                {
                    return Ok(await GenerateJWTTokenAsync(dbUser, storedToken));
                }
                else
                {
                    return Ok(await GenerateJWTTokenAsync(dbUser, null));
                }
            }
        }
        private async Task<AuthResultDTO> GenerateJWTTokenAsync(IdentityUser user, RefreshToken rToken)
        {
            var authClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var role in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }
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
                UserId = user.Id,
                DateAdded = DateTime.UtcNow,
                DateExpired = DateTime.UtcNow.AddMonths(6),
                RefrToken = Guid.NewGuid().ToString() + "-" + Guid.NewGuid().ToString(),
            };
            await _context.RefreshTokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync();
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
