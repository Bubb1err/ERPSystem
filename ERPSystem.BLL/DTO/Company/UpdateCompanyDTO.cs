using System.ComponentModel.DataAnnotations;


namespace ERPSystem.BLL.DTO.Company
{
    public class UpdateCompanyDTO
    {
        public string OwnerId { get; set; }
        [Required]
        [MinLength(1)]
        public string Name { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
