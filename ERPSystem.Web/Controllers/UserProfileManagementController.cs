using ERPSystem.BLL.Commands.CompanyCommands;
using ERPSystem.BLL.Commands.UserCommands;
using ERPSystem.BLL.DTO.Auth;
using ERPSystem.BLL.DTO.Company;
using ERPSystem.BLL.Queries.UserQueries;
using ERPSystem.Resources;
using ERPSystem.Web.AuthorizationHandling;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ERPSystem.Web.Controllers
{
    [ApiController]
    public class UserProfileManagementController : ControllerBase
    {
        private readonly IMediator _mediator;
        public UserProfileManagementController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize]
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId)) { return BadRequest("User id could not be empty"); }
            var getUserProfileQuery = new GetProfileQuery(userId);
            var result = await _mediator.Send(getUserProfileQuery);
            return StatusCode((int)result.StatusCode, result.Object);
        }
        [HttpPost("company")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> UpdateCompany([FromForm]UpdateCompanyDTO updateCompanyDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var updateCompanyCommand = new UpdateCompanyCommand(updateCompanyDto);
            var result = await _mediator.Send(updateCompanyCommand);
            return StatusCode((int)result.StatusCode, result);
        }
        [HttpPost("add-employee")]
        [AuthorizePermission(Permission.AddNewEmployee)]
        public async Task<IActionResult> AddEmployee([FromBody]RegisterEmployeeDTO registerEmployeeDTO)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var addEmployeeCommand = new EmployeeRegisterCommand(registerEmployeeDTO);
            var result = await _mediator.Send(addEmployeeCommand);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
