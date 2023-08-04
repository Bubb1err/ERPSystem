using ERPSystem.BLL.DTO.Auth;
using ERPSystem.DataAccess.Entities.Auth;
using ERPSystem.DataAccess.Entities.UserEntities;

namespace ERPSystem.BLL.Services.AuthService
{
    public interface IAuthenticationService
    {
        Task<AuthResultDTO> GenerateJWTTokenAsync(User user, RefreshToken rToken);
    }
}
