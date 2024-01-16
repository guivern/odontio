using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Odontio.Application.Authentication.Queries.Login;

namespace Odontio.API.Controllers;

public class AuthController(IMediator mediator) : ApiControllerBase
{
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginQuery query)
    {
        var result = await mediator.Send(query);

        return result.Match<IActionResult>(
            result => Ok(result),
            errors => Problem(errors)
        );
    }
}