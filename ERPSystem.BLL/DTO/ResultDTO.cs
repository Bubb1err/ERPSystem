using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ERPSystem.BLL.DTO
{
    public class ResultDTO<ResultObject> where ResultObject : class
    {
        public HttpStatusCode StatusCode {  get; set; }
        public ResultObject? Object { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        public List<IdentityError> IdentityErrors { get; set; } = new List<IdentityError>();
    }
}
