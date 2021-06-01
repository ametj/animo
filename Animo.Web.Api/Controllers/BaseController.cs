using Microsoft.AspNetCore.Mvc;

namespace Animo.Web.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public abstract class BaseController : ControllerBase
    {
    }
}