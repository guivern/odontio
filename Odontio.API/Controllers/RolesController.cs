using Odontio.Application.Roles.Queries.GetRoles;

namespace Odontio.API.Controllers;

[Route("api/v1/[controller]")]
public class RolesController(IMediator mediator, IMapper mapper) : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetRoles(CancellationToken cancellationToken)
    {
        var query = new GetRolesQuery();
        var result = await mediator.Send(query, cancellationToken);
        return result.Match<IActionResult>(
            result => Ok(result),
            errors => Problem(errors)
        );
    }
}