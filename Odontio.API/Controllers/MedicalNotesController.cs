using Odontio.API.Contracts.Appointments;
using Odontio.Application.MedicalNotes.Commands.CreateMedicalNote;
using Odontio.Application.MedicalNotes.Commands.DeleteMedicalNote;
using Odontio.Application.MedicalNotes.Commands.UpdateMedicalNote;
using Odontio.Application.MedicalNotes.Queries.GetMedicalNoteById;

namespace Odontio.API.Controllers;

[Route("api/v1/Workspaces/{workspaceId}/patients/{patientId}/appointments/{appointmentId}/[controller]")]
public class MedicalNotesController(IMediator mediator) : ApiControllerBase
{
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(long id, long workspaceId, long patientId,
        CancellationToken cancellationToken)
    {
        var query = new GetMedicalNoteByIdQuery
        {
            Id = id,
            WorkspaceId = workspaceId,
            PatientId = patientId
        };
        
        var result = await mediator.Send(query, cancellationToken);

        return result.Match<IActionResult>(
            result => Ok(result),
            errors => Problem(errors)
        );
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateMedicalNoteRequest request, long workspaceId, long patientId,
        long appointmentId, CancellationToken cancellationToken)
    {
        var command = new CreateMedicalNoteCommand
        {
            WorkspaceId = workspaceId,
            PatientId = patientId,
            AppointmentId = appointmentId,
            PatientTreatmentId = request.PatientTreatmentId,
            Description = request.Description
        };

        var result = await mediator.Send(command, cancellationToken);

        return result.Match<IActionResult>(
            result => CreatedAtAction(nameof(GetById),
                new { id = result.Id, workspaceId, patientId, appointmentId = appointmentId }, result),
            errors => Problem(errors)
        );
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> Update(long id, UpdateMedicalNoteRequest request, long workspaceId,
        long patientId, long appointmentId, CancellationToken cancellationToken)
    {
        if (id != request.Id) return BadRequest();

        var command = new UpdateMedicalNoteCommand
        {
            Id = id,
            WorkspaceId = workspaceId,
            PatientId = patientId,
            AppointmentId = appointmentId,
            PatientTreatmentId = request.PatientTreatmentId,
            Description = request.Description,
            Observations = request.Observations
        };

        var result = await mediator.Send(command, cancellationToken);

        return result.Match<IActionResult>(
            result => Ok(result),
            errors => Problem(errors)
        );
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id, long workspaceId, long patientId, long appointmentId,
        CancellationToken cancellationToken)
    {
        var command = new DeleteMedicalNoteCommand
        {
            Id = id,
            WorkspaceId = workspaceId,
            PatientId = patientId,
            AppointmentId = appointmentId
        };
        var result = await mediator.Send(command, cancellationToken);

        return result.Match<IActionResult>(
            result => NoContent(),
            errors => Problem(errors)
        );
    }
}