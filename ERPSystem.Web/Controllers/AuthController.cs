using ERPSystem.BLL.Commands.UserCommands;
using ERPSystem.BLL.DTO.Auth;
using ERPSystem.DataAccess.Entities.Auth;
using ERPSystem.DataAccess.Entities.UserEntities;
using ERPSystem.DataAccess.Repositories.Interfaces;
using ERPSystem.DataAccess.Repositories.Interfaces.Auth;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ERPSystem.Web.Controllers
{
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IMediator _mediator;
        private readonly ERPSystem.BLL.Services.AuthService.IAuthenticationService _authenticationService;
        public AuthController(UserManager<User> userManager,
            IMediator mediator,
            ERPSystem.BLL.Services.AuthService.IAuthenticationService authenticationService,
            IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _mediator = mediator;
            _authenticationService = authenticationService;
            _unitOfWork = unitOfWork;
            _refreshTokenRepository = _unitOfWork.GetRepository<RefreshToken, IRefreshTokenRepository>(hasCustomRepository: true);

        }
        //only for "admin" users
        [HttpPost("signup")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromForm]AdminRegisterDTO registerDto)
        {
            if(registerDto.Password != registerDto.ConfirmPassword) return BadRequest("Passwords mismatch.");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var adminRegisterCommand = new AdminRegisterCommand(registerDto);
            var result = await _mediator.Send(adminRegisterCommand);

            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return Ok(result);
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
                var token = await _authenticationService.GenerateJWTTokenAsync(userExist, null);
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
            var storedToken = await _refreshTokenRepository.GetFirstOrDefaultAsync(x => x.RefrToken == refreshToken);
            if (storedToken == null) return Unauthorized();
            var dbUser = await _userManager.FindByIdAsync(storedToken.IdentityUserId);
            try
            {
                return Ok(await _authenticationService.GenerateJWTTokenAsync(dbUser, storedToken));
            }
            catch (SecurityTokenExpiredException)
            {
                if (storedToken.DateExpired >= DateTime.UtcNow)
                {
                    return Ok(await _authenticationService.GenerateJWTTokenAsync(dbUser, storedToken));
                }
                else
                {
                    return Ok(await _authenticationService.GenerateJWTTokenAsync(dbUser, null));
                }
            }
        }
    }
}
