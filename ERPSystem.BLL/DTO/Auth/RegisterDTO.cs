using System.ComponentModel.DataAnnotations;

namespace ERPSystem.BLL.DTO.Auth
{
    public class RegisterDTO
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string ConfirmPassword { get; set; }
    }
}
