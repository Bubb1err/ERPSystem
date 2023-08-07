using AutoMapper;
using ERPSystem.BLL.Commands.UserCommands;
using ERPSystem.BLL.DTO.Auth;
using ERPSystem.DataAccess.Entities.UserEntities;
using ERPSystem.DataAccess.Repositories.Interfaces;
using ERPSystem.Resources;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace ERPSystem.BLL.Handlers.AuthHandlers
{
    public class AdminRegisterHandler : IRequestHandler<AdminRegisterCommand, RegisterAdminResultDto>
    {
        private readonly UserManager<User> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminRegisterHandler(
            UserManager<User> userManager,
            IUnitOfWork unitOfWork,
            IMapper mapper, 
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _roleManager = roleManager;
        }
        public async Task<RegisterAdminResultDto> Handle(AdminRegisterCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.AdminRegisterDTO.Email);
            if (user != null)
            {
                return new RegisterAdminResultDto
                {
                    StatusCode = System.Net.HttpStatusCode.Conflict,
                    Errors = new List<string> { "User already exists." }
                };
            }

            var userProfile = new UserProfile
            {
                Name = request.AdminRegisterDTO.Name,
                Surname = request.AdminRegisterDTO.Surname,
            };

            var newUser = new User
            {
                UserName = request.AdminRegisterDTO.Email,
                Email = request.AdminRegisterDTO.Email,
                UserProfile = userProfile,
            };

            userProfile.User = newUser;
            userProfile.UserId = newUser.Id;

            var result = await _userManager.CreateAsync(newUser, request.AdminRegisterDTO.Password);
            if (!result.Succeeded)
            {
                return new RegisterAdminResultDto
                {
                    IdentityErrors = result.Errors.ToList(),
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };
            }
            var company = new Company { Name = request.AdminRegisterDTO.CompanyName, Owner = newUser, OwnerId = newUser.Id };
            newUser.Company = company;
            newUser.CompanyId = company.Id;
            await _unitOfWork.SaveChangesAsync();

            var permissions = Enum.GetNames(typeof(ERPSystem.Resources.Permission));
            var claims = new List<Claim>();

            foreach (var permission in permissions)
            {
                claims.Add(new Claim(ClaimTypes.AuthorizationDecision, permission));
            }
            await _userManager.AddClaimsAsync(newUser, claims);
            if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
            }
            await _userManager.AddToRoleAsync(newUser, UserRoles.Admin);
            var registerResultDto = _mapper.Map<RegisterAdminResultDto>(newUser);
            registerResultDto.StatusCode = System.Net.HttpStatusCode.OK;
            return registerResultDto;           
        }
    }
}
