
using ERPSystem.BLL.DTO;
using ERPSystem.BLL.DTO.Company;
using MediatR;

namespace ERPSystem.BLL.Commands.CompanyCommands
{
    public class UpdateCompanyCommand : IRequest<ResultDTO<UpdateCompanyDTO>>
    {
        public UpdateCompanyDTO UpdateCompanyDTO { get; set; }
        public UpdateCompanyCommand(UpdateCompanyDTO updateCompanyDTO)
        {
            this.UpdateCompanyDTO = updateCompanyDTO;
        }

    }
}
