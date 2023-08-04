
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ERPSystem.DataAccess.Entities.UserEntities;

namespace ERPSystem.DataAccess.Entities.Auth
{
    public class RefreshToken
    {
        [Key]
        public int Id { get; set; }
        public string RefrToken { get; set; }
        public string JwtId { get; set; }
        public bool IsRevoked { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime DateExpired { get; set; }
        public string IdentityUserId { get; set; }
        [ForeignKey(nameof(IdentityUserId))]
        public User User { get; set; }
    }
}
