using ERPSystem.DataAccess.Entities.UserEntities;

namespace ERPSystem.BLL.DTO.User
{
    public class UserProfileDTO
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public List<Skill> Skills { get; set; }
        public string JobTitle { get; set; }
        public DateTime Birthday { get; set; }

    }
}
