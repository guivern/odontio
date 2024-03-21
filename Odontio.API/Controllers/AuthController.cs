using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Odontio.API.Contracts.Authentication;
using Odontio.Application.Authentication.Commands.ChangePassword;
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
    
    [HttpPatch("ChangePassword")]
    public async Task<IActionResult> Login(ChangePasswordRequest request)
    {
        var command = mapper.Map<ChangePasswordCommand>(request);
        var result = await mediator.Send(command);

        return result.Match<IActionResult>(
            result => Ok(result),
            errors => Problem(errors)
        );
    } 
}