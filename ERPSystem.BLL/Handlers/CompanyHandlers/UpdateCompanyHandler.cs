
using ERPSystem.BLL.Commands.CompanyCommands;
using ERPSystem.BLL.DTO;
using ERPSystem.BLL.DTO.Company;
using ERPSystem.BLL.Services.LoggerManagerService;
using ERPSystem.DataAccess.Entities.UserEntities;
using ERPSystem.DataAccess.Repositories.Interfaces;
using ERPSystem.DataAccess.Repositories.Interfaces.UserRelatedRepositories;
using MediatR;

namespace ERPSystem.BLL.Handlers.CompanyHandlers
{
    public class UpdateCompanyHandler : IRequestHandler<UpdateCompanyCommand, ResultDTO<UpdateCompanyDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly ILoggerManager _logger;
        public UpdateCompanyHandler(
            IUnitOfWork unitOfWork, 
            ILoggerManager loggerManager)
        {
            _unitOfWork = unitOfWork;
            _userRepository = _unitOfWork.GetRepository<User, IUserRepository>(hasCustomRepository: true);
            _logger = loggerManager;
        }
        public async Task<ResultDTO<UpdateCompanyDTO>> Handle(UpdateCompanyCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetFirstOrDefaultAsync(u => u.Id == request.UpdateCompanyDTO.OwnerId);
            if (user == null)
            {
                _logger.LogError("User was not found.");
                return new ResultDTO<UpdateCompanyDTO>
                {
                    Errors = new List<string> { "User was not found." },
                    StatusCode = System.Net.HttpStatusCode.NotFound
                };
            }
            if (user.Company == null) 
            {
                _logger.LogError("Company was not found.");
                return new ResultDTO<UpdateCompanyDTO>
                {
                    Errors = new List<string> { "Company was not found." },
                    StatusCode = System.Net.HttpStatusCode.NotFound
                };
            }

            user.Company.Name = request.UpdateCompanyDTO.Name;
            user.Company.Description = request.UpdateCompanyDTO.Description;
            _userRepository.Update(user);
            await _unitOfWork.SaveChangesAsync();
            return new ResultDTO<UpdateCompanyDTO>
            {
                Object = new UpdateCompanyDTO
                {
                    Name = user.Company.Name,
                    Description = user.Company.Description,
                    OwnerId = user.Id
                },
                StatusCode = System.Net.HttpStatusCode.OK
            };
        }
    }
}
