using AutoMapper;
using ERPSystem.BLL.DTO.Auth;
using ERPSystem.DataAccess.Entities.UserEntities;

namespace ERPSystem.BLL.Mapping.UserMappings
{
    public class RegisterResultProfile : Profile
    {
        public RegisterResultProfile()
        {
            CreateMap<User, RegisterAdminResultDto>().ForMember(x => x.CompanyName,
                m => m.MapFrom(u => u.Company.Name))
               .ForMember(x => x.Email,
               m => m.MapFrom(u => u.Email))
               .ForMember(x => x.UserId,
               m => m.MapFrom(u => u.Id));
        }

    }
}
