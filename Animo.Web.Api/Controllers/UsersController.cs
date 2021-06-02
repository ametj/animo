using Animo.Web.Core.Dto;
using Animo.Web.Core.Models.Permissions;
using Animo.Web.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Animo.Web.Api.Controllers
{
    [Authorize(Policy = DefaultPermissions.PermissionNameForAdminAccess)]
    public class UsersController : BaseController
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<UserListDto>>> GetUsers([FromQuery] PagedListRequest request)
        {
            return Ok(await _userService.GetUsersAsync(request));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetUserForCreateOrUpdate>> GetUsers(int id)
        {
            var user = await _userService.GetUserForCreateOrUpdateAsync(id);

            return user == null ? NotFound() : Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult> PostUsers([FromBody] CreateOrUpdateUser user)
        {
            var result = await _userService.AddUserAsync(user);

            return ProcessIdentityValidation(result) ? Created(Url.Action("PostUsers"), result) : ValidationProblem();
        }

        [HttpPut]
        public async Task<ActionResult> PutUsers([FromBody] CreateOrUpdateUser user)
        {
            var result = await _userService.EditUserAsync(user);

            if (result == null) return NotFound();

            return ProcessIdentityValidation(result) ? Ok() : ValidationProblem();
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteUsers(int id)
        {
            var result = await _userService.RemoveUserAsync(id);

            if (result == null) return NotFound();

            return ProcessIdentityValidation(result) ? NoContent() : ValidationProblem();
        }
    }
}