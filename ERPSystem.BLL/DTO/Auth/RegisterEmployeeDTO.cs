using ERPSystem.Resources;
using System.ComponentModel.DataAnnotations;

namespace ERPSystem.BLL.DTO.Auth
{
    public class RegisterEmployeeDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string ConfirmPassword { get; set; }
        [Required]
        public Permission[] Permissions { get; set; }
        [Required]
        [MinLength(1)]
        public string Name { get; set; }
        [Required]
        [MinLength(1)]
        public string Surname { get; set; }
        public string JobPosition { get; set; } = string.Empty;
    }
}
