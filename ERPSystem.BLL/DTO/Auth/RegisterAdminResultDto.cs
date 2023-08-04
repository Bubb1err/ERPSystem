
using Microsoft.AspNetCore.Identity;
using System.Net;

namespace ERPSystem.BLL.DTO.Auth
{
    public class RegisterAdminResultDto
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string CompanyName { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        public List<IdentityError> IdentityErrors { get; set; } = new List<IdentityError>();
    }
}
