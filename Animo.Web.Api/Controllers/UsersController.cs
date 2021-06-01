using Animo.Web.Core.Dto;
using Animo.Web.Core.Models.Permissions;
using Animo.Web.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Animo.Web.Api.Controllers
{
    //[Authorize(Policy = DefaultPermissions.PermissionNameForAdminAccess)]
    public class UsersController : BaseController
    {
        private readonly IUserService _userAppService;

        public UsersController(IUserService userAppService)
        {
            _userAppService = userAppService;
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<UserListDto>>> GetUsers([FromQuery] PagedListRequest request)
        {
            return Ok(await _userAppService.GetUsersAsync(request));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetUserForCreateOrUpdate>> GetUsers(int id)
        {
            var getUserForCreateOrUpdateOutput = await _userAppService.GetUserForCreateOrUpdateAsync(id);

            return Ok(getUserForCreateOrUpdateOutput);
        }

        [HttpPost]
        public async Task<ActionResult> PostUsers([FromBody] CreateOrUpdateUser user)
        {
            var identityResult = await _userAppService.AddUserAsync(user);

            if (identityResult.Succeeded)
            {
                return Created(Url.Action("PostUsers"), identityResult);
            }
            return BadRequest(identityResult.Errors.Select(e => new IdValueDto<string>(e.Code, e.Description)));
        }

        [HttpPut]
        public async Task<ActionResult> PutUsers([FromBody] CreateOrUpdateUser user)
        {
            var identityResult = await _userAppService.EditUserAsync(user);

            if (identityResult.Succeeded)
            {
                return Ok();
            }

            return BadRequest(identityResult.Errors.Select(e => new IdValueDto<string>(e.Code, e.Description)));
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteUsers(int id)
        {
            var identityResult = await _userAppService.RemoveUserAsync(id);

            if (identityResult.Succeeded)
            {
                return NoContent();
            }

            return BadRequest(identityResult.Errors.Select(e => new IdValueDto<string>(e.Code, e.Description)));
        }
    }
}