using ERPSystem.BLL.DTO.Auth;
using MediatR;

namespace ERPSystem.BLL.Commands.UserCommands
{
    public class AdminRegisterCommand : IRequest<RegisterAdminResultDto>
    {
        public AdminRegisterDTO AdminRegisterDTO { get; set; }
        public AdminRegisterCommand(AdminRegisterDTO adminRegisterDTO)
        {
                this.AdminRegisterDTO = adminRegisterDTO;
        }
    }
}
