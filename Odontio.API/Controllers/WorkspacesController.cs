using Odontio.Application.Appointments.Queries.GetAppointmentsByWorkspace;
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

    [HttpGet("{workspaceId}/Appointments")]
    public async Task<IActionResult> GetAppointments(long workspaceId, PaginationQueryParams pagination,
        CancellationToken cancellationToken)
    {
        var query = new GetAppointmentsByWorkspaceQuery
        {
            WorkspaceId = workspaceId,
            Page = pagination.Page,
            PageSize = pagination.PageSize,
            Filter = pagination.Filter,
            OrderBy = pagination.OrderBy
        };

        var result = await mediator.Send(query, cancellationToken);

        Response.AddPaginationHeader(result.PageNumber, result.PageSize, result.TotalCount, result.TotalPages);
        return Ok(result);
    }
}