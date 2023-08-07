using System.ComponentModel.DataAnnotations;

namespace ERPSystem.BLL.DTO.Auth
{
    public class AdminRegisterDTO
    {
        [Required]
        [MinLength(1)]
        public string Name { get; set; }
        [Required]
        [MinLength(1)]
        public string Surname { get; set; }
        [EmailAddress]
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string ConfirmPassword { get; set; }
        [Required]
        [MinLength(1)]
        public string CompanyName { get; set; }
    }
}
