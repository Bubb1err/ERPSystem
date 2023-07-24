using System.ComponentModel.DataAnnotations;

namespace ERPSystem.BLL.DTO.Auth
{
    public class LoginDTO
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
