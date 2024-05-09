using Microsoft.AspNetCore.Authorization;
using Odontio.Application.Authentication.Queries.Login;

namespace Odontio.API.Controllers;

public class AuthController(IMediator mediator, IMapper mapper) : ApiControllerBase
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