using Odontio.API.Contracts.Appointments;
using Odontio.Application.Appointments.Commands.CreateAppointment;
using Odontio.Application.Appointments.Commands.DeleteAppointment;
using Odontio.Application.Appointments.Commands.UpdateAppointment;
using Odontio.Application.Appointments.Queries.GetAppointmentById;
using Odontio.Application.Appointments.Queries.GetAppointments;

namespace Odontio.API.Controllers;

[Route("api/v1/Workspaces/{workspaceId}/patients/{patientId}/[controller]")]
public class AppointmentsController(IMediator mediator, IMapper mapper) : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAppointmentsByPatient(long workspaceId, long patientId,
        PaginationQueryParams pagination, CancellationToken cancellationToken)
    {
        var query = new GetAppointmentsQuery
        {
            PatientId = patientId,
            WorkspaceId = workspaceId,
            Page = pagination.Page,
            PageSize = pagination.PageSize,
            OrderBy = pagination.OrderBy
        };
        
        var result = await mediator.Send(query, cancellationToken);
        return result.Match<IActionResult>(
            result =>
            {
                Response.AddPaginationHeader(result.PageNumber, result.PageSize, result.TotalCount, result.TotalPages);
                return Ok(result);
            },
            errors => Problem(errors)
        );
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAppointmentById(long id, long workspaceId, long patientId, CancellationToken cancellationToken)
    {
        var query = new GetAppointmentByIdQuery { Id = id, WorkspaceId = workspaceId, PatientId = patientId };
        var result = await mediator.Send(query, cancellationToken);

        return result.Match<IActionResult>(
            result => Ok(result),
            errors => Problem(errors)
        );
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateAppointment(long workspaceId, long patientId, CreateAppointmentRequest request,
        CancellationToken cancellationToken)
    {
        var command = mapper.Map<CreateAppointmentCommand>(request);
        command.WorkspaceId = workspaceId;
        command.PatientId = patientId;

        var result = await mediator.Send(command, cancellationToken);

        return result.Match<IActionResult>(
            result => CreatedAtAction(nameof(GetAppointmentById),
                new GetAppointmentByIdQuery { Id = result.Id, WorkspaceId = workspaceId, PatientId = patientId }, result),
            errors => Problem(errors)
        );
    }
    
    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateAppointment(long id, long workspaceId, long patientId, UpdateAppointmentRequest request,
        CancellationToken cancellationToken)
    {
        var command = mapper.Map<UpdateAppointmentCommand>(request);
        command.Id = id;
        command.WorkspaceId = workspaceId;
        command.PatientId = patientId;

        var result = await mediator.Send(command, cancellationToken);

        return result.Match<IActionResult>(
            result => Ok(result),
            errors => Problem(errors)
        );
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAppointment(long id, long workspaceId, long patientId, CancellationToken cancellationToken)
    {
        var command = new DeleteAppointmentCommand { Id = id, WorkspaceId = workspaceId, PatientId = patientId };
        var result = await mediator.Send(command, cancellationToken);

        return result.Match<IActionResult>(
            result => Ok(),
            errors => Problem(errors)
        );
    }
}