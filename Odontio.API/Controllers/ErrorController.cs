using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Odontio.Application.Common.Exceptions;

namespace Odontio.API.Controllers;

[Route("/error")]
[ApiExplorerSettings(IgnoreApi = true)]
public class ErrorController : ControllerBase
{
    public IActionResult Error()
    {
        Exception? exception = HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error;
        
        if (exception.GetType() == typeof(UnauthorizedException))
            return Problem(title: exception.Message, statusCode: 403);

        return Problem(title: exception?.Message);
    }
}