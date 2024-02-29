using Odontio.Application.ScheduledVisits.Queries.GetScheduledVisitsByWorkspace;

namespace Odontio.API.Controllers;

[Route("api/v1/[controller]")]
public class WorkspacesController(IMediator mediator, IMapper mapper) : ApiControllerBase
{
    [HttpGet("{workspaceId}/ScheduledVisits")]
    public async Task<IActionResult> GetScheduledVisits(long workspaceId, CancellationToken cancellationToken)
    {
        var query = new GetScheduledVisitsByWorkspaceQuery
        {
            WorkspaceId = workspaceId
        };

        var result = await mediator.Send(query, cancellationToken);

        return Ok(result);
    }
}