using AutoMapper;
using ERPSystem.BLL.Commands.UserCommands;
using ERPSystem.BLL.DTO.Auth;
using ERPSystem.DataAccess.Entities.UserEntities;
using ERPSystem.DataAccess.Repositories.Interfaces;
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
        public AdminRegisterHandler(
            UserManager<User> userManager,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _mapper = mapper;

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
            var newUser = new User { UserName = request.AdminRegisterDTO.Email, Email = request.AdminRegisterDTO.Email };
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
            var registerResultDto = _mapper.Map<RegisterAdminResultDto>(newUser);
            registerResultDto.StatusCode = System.Net.HttpStatusCode.OK;
            return registerResultDto;           
        }
    }
}
