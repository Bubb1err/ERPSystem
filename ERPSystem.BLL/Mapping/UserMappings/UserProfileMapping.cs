using AutoMapper;
using ERPSystem.BLL.DTO.User;
using ERPSystem.DataAccess.Entities.UserEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPSystem.BLL.Mapping.UserMappings
{
    public class UserProfileMapping : Profile
    {
        public UserProfileMapping()
        {
            CreateMap<UserProfile,  UserProfileDTO>()
                .ForMember(u => u.Email, 
                m => m.MapFrom(e => e.User.Email));
        }
    }
}
