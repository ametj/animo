using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Animo.Web.Core.ActionFilters
{
    public class BadRequestLogging : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            var loggerFactory = context.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger(context.ActionDescriptor.DisplayName);

            if (context.Result is BadRequestObjectResult result)
            {
                logger.LogInformation(JsonSerializer.Serialize((ValidationProblemDetails)result.Value));
            }
        }
    }
}