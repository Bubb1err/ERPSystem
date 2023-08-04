
using Microsoft.AspNetCore.Identity;

namespace ERPSystem.DataAccess.Entities.UserEntities
{
    public class User : IdentityUser
    {
        public UserProfile UserProfile { get; set; }
        public int? CompanyId { get; set; }
        public Company? Company { get; set; }

    }
}
