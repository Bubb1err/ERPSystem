

using ERPSystem.BLL.DTO;
using ERPSystem.BLL.DTO.User;
using MediatR;

namespace ERPSystem.BLL.Queries.UserQueries
{
    public class GetProfileQuery : IRequest<ResultDTO<UserProfileDTO>>
    {
        public string UserId { get; set; }
        public GetProfileQuery(string userId)
        {
            this.UserId = userId;
        }
    }
}
