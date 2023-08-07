

using ERPSystem.BLL.DTO;
using ERPSystem.BLL.DTO.Auth;
using MediatR;

namespace ERPSystem.BLL.Commands.UserCommands
{
    public class EmployeeRegisterCommand : IRequest<ResultDTO<EmployeeRegisterResultDTO>>
    {
        public RegisterEmployeeDTO RegisterEmployee { get; set; }
        public EmployeeRegisterCommand(RegisterEmployeeDTO registerEmployeeDTO)
        {
            this.RegisterEmployee = registerEmployeeDTO;
        }
    }
}
