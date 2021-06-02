using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Animo.Web.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public abstract class BaseController : ControllerBase
    {
        protected bool ProcessIdentityValidation(IdentityResult result)
        {
            foreach (var e in result.Errors)
            {
                ModelState.AddModelError(e.Code, e.Description);
            }

            return result.Succeeded;
        }
    }
}