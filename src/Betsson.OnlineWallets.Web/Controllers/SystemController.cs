using Betsson.OnlineWallets.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Betsson.OnlineWallets.Web.UnitTests")]
// needed for mocking
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
namespace Betsson.OnlineWallets.Web.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class SystemController : ControllerBase
    {
        public SystemController()
        {
            HttpContextNew = HttpContext;
        }

        internal virtual HttpContext HttpContextNew { get; set; }

        public IActionResult Error()
        {
            IExceptionHandlerPathFeature? exceptionHandlerPathFeature = HttpContextNew.Features.Get<IExceptionHandlerPathFeature>();

            if (exceptionHandlerPathFeature?.Error is InsufficientBalanceException)
            {
                Exception? exception = exceptionHandlerPathFeature?.Error;
                return Problem(exception?.StackTrace, statusCode: 400, title: exception?.Message, type: nameof(InsufficientBalanceException));
            }

            return Problem();
        }

    }
}
