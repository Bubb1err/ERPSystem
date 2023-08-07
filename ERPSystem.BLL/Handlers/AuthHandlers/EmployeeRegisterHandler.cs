using ERPSystem.BLL.Commands.UserCommands;
using ERPSystem.BLL.DTO.Auth;
using ERPSystem.BLL.DTO;
using MediatR;
using Microsoft.AspNetCore.Identity;
using ERPSystem.DataAccess.Entities.UserEntities;
using AutoMapper;
using System.Security.Claims;
using ERPSystem.Resources;

namespace ERPSystem.BLL.Handlers.AuthHandlers
{
    public class EmployeeRegisterHandler : IRequestHandler<EmployeeRegisterCommand, ResultDTO<EmployeeRegisterResultDTO>>
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly RoleManager<IdentityRole> _roleManager;

        public EmployeeRegisterHandler(
            UserManager<User> userManager,
            IMapper mapper,
            RoleManager<IdentityRole> roleManager)
        {
            this._userManager = userManager;
            this._mapper = mapper;
            this._roleManager = roleManager;
        }
        public async Task<ResultDTO<EmployeeRegisterResultDTO>> Handle(EmployeeRegisterCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.RegisterEmployee.Email);
            if (user != null)
            {
                return new ResultDTO<EmployeeRegisterResultDTO>
                {
                    StatusCode = System.Net.HttpStatusCode.Conflict,
                    Errors = new List<string> { "User already exists." }
                };
            }

            var userProfile = new UserProfile
            {
                Name = request.RegisterEmployee.Name,
                Surname = request.RegisterEmployee.Surname,
                JobPosition = request.RegisterEmployee.JobPosition,
            };

            var newUser = new User 
            { 
                UserName = request.RegisterEmployee.Email, 
                Email = request.RegisterEmployee.Email, 
                UserProfile = userProfile 
            };

            userProfile.User = newUser;
            userProfile.UserId = newUser.Id;

            var result = await _userManager.CreateAsync(newUser, request.RegisterEmployee.Password);
            if (!result.Succeeded)
            {
                return new ResultDTO<EmployeeRegisterResultDTO>
                {
                    IdentityErrors = result.Errors.ToList(),
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };
            }

            var claims = new List<Claim>();

            foreach (var permission in request.RegisterEmployee.Permissions)
            {
                claims.Add(new Claim(ClaimTypes.AuthorizationDecision, permission.ToString()));
            }
            await _userManager.AddClaimsAsync(newUser, claims);

            if (!await _roleManager.RoleExistsAsync(UserRoles.Employee))
            {
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Employee));
            }
            await _userManager.AddToRoleAsync(newUser, UserRoles.Employee);

            var userResult = _mapper.Map<EmployeeRegisterResultDTO>(newUser);

            return new ResultDTO<EmployeeRegisterResultDTO>
            {
                Object = userResult,
                StatusCode = System.Net.HttpStatusCode.OK
            };
        }
    }
}
