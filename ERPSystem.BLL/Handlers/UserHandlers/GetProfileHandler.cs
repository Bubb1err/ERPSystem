

using AutoMapper;
using ERPSystem.BLL.DTO;
using ERPSystem.BLL.DTO.User;
using ERPSystem.BLL.Queries.UserQueries;
using ERPSystem.DataAccess.Entities.UserEntities;
using ERPSystem.DataAccess.Repositories.Interfaces;
using ERPSystem.DataAccess.Repositories.Interfaces.UserRelatedRepositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ERPSystem.BLL.Handlers.UserHandlers
{
    public class GetProfileHandler : IRequestHandler<GetProfileQuery, ResultDTO<UserProfileDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserProfileRepository _profileRepository;

        public GetProfileHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _profileRepository = _unitOfWork.GetRepository<UserProfile, IUserProfileRepository>(hasCustomRepository: true);
        }
        public async Task<ResultDTO<UserProfileDTO>> Handle(GetProfileQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.UserId))
            {
                return new ResultDTO<UserProfileDTO>
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Errors = new List<string>() { "User id could not be null" }
                };
            }

            var userProfile = await _profileRepository.GetFirstOrDefaultAsync(
                u => u.UserId == request.UserId,
                s => s.Include(u => u.User).Include(u => u.Skills));

            if (userProfile == null)
            {
                return new ResultDTO<UserProfileDTO>
                {
                    StatusCode = System.Net.HttpStatusCode.NotFound,
                    Errors = new List<string> { "User was not found." }
                };
            }

            var userProfileDto = _mapper.Map<UserProfileDTO>(userProfile);
            return new ResultDTO<UserProfileDTO>
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Object = userProfileDto
            };
        }
    }
}
