using Odontio.API.Contracts.ScheduledVisits;
using Odontio.Application.Common.Helpers;
using Odontio.Application.ScheduledVisits.Commands.CreateScheduledVisit;
using Odontio.Application.ScheduledVisits.Commands.DeleteScheduledVisit;
using Odontio.Application.ScheduledVisits.Commands.UpdateScheduledVisit;
using Odontio.Application.ScheduledVisits.Queries.GetScheduledVisitsByPatient;
using Odontio.Application.ScheduledVisits.Queries.GetScheduleVisitById;

namespace Odontio.API.Controllers;

[Route("api/v1/Workspaces/{workspaceId}/patients/{patientId}/[controller]")]
public class ScheduledVisitsController(IMediator mediator, IMapper mapper) : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllByPatient(long workspaceId, long patientId, [FromQuery] DateOnly? startDate,
        [FromQuery] DateOnly? endDate, CancellationToken cancellationToken)
    {
        var query = new GetScheduledVisitsQuery
        {
            PatientId = patientId,
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

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(long workspaceId, long patientId, long id, CancellationToken cancellationToken)
    {
        var query = new GetScheduleVisitByIdQuery
        {
            PatientId = patientId,
            WorkspaceId = workspaceId,
            Id = id
        };

        var result = await mediator.Send(query, cancellationToken);

        return result.Match<IActionResult>(
            result => Ok(result),
            errors => Problem(errors)
        );
    }

    [HttpPost]
    public async Task<IActionResult> Create(long workspaceId, long patientId, CreateScheduledVisitRequest request,
        CancellationToken cancellationToken)
    {
        var command = mapper.Map<CreateScheduleVisitCommand>(request);

        command.PatientId = patientId;
        command.WorkspaceId = workspaceId;

        var result = await mediator.Send(command, cancellationToken);

        return result.Match<IActionResult>(
            result => Ok(result),
            errors => Problem(errors)
        );
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> Update(long workspaceId, long patientId, long id,
        UpdateScheduledVisitRequest request,
        CancellationToken cancellationToken)
    {
        var command = mapper.Map<UpdateScheduledVisitCommand>(request);

        command.PatientId = patientId;
        command.WorkspaceId = workspaceId;
        command.Id = id;

        var result = await mediator.Send(command, cancellationToken);

        return result.Match<IActionResult>(
            result => Ok(result),
            errors => Problem(errors)
        );
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long workspaceId, long patientId, long id,
        CancellationToken cancellationToken)
    {
        var command = new DeleteScheduledVisitCommand
        {
            PatientId = patientId,
            WorkspaceId = workspaceId,
            Id = id
        };

        var result = await mediator.Send(command, cancellationToken);

        return result.Match<IActionResult>(
            result => Ok(),
            errors => Problem(errors)
        );
    }
}