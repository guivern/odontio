using Odontio.Application.Common.Helpers;
using Odontio.Application.ScheduledVisits.Queries.GetScheduledVisitsByWorkspace;

namespace Odontio.API.Controllers;

[Route("api/v1/[controller]")]
public class WorkspacesController(IMediator mediator, IMapper mapper) : ApiControllerBase
{
    [HttpGet("{workspaceId}/ScheduledVisits")]
    public async Task<IActionResult> GetScheduledVisits(long workspaceId, DateOnly? startDate, DateOnly? endDate,
        CancellationToken cancellationToken)
    {
        var query = new GetScheduledVisitsByWorkspaceQuery
        {
            WorkspaceId = workspaceId,
            DateRange = new DateRange
            {
                StartDate = startDate,
                EndDate = endDate
            }
        };

        var result = await mediator.Send(query, cancellationToken);

        return Ok(result);
    }
}