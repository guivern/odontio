using Odontio.API.Contracts.Appointments;
using Odontio.Application.MedicalRecords.Commands.CreateMedicalRecord;
using Odontio.Application.MedicalRecords.Commands.DeleteMedicalRecord;
using Odontio.Application.MedicalRecords.Commands.UpdateMedicalRecord;
using Odontio.Application.MedicalRecords.Queries.GetMedicalRecordById;

namespace Odontio.API.Controllers;

[Route("api/v1/Workspaces/{workspaceId}/patients/{patientId}/appointments/{appointmentId}/[controller]")]
public class MedicalRecordsController(IMediator mediator) : ApiControllerBase
{
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(long id, long workspaceId, long patientId,
        CancellationToken cancellationToken)
    {
        var query = new GetMedicalRecordByIdQuery
            { Id = id, WorkspaceId = workspaceId, PatientId = patientId };
        var result = await mediator.Send(query, cancellationToken);

        return result.Match<IActionResult>(
            result => Ok(result),
            errors => Problem(errors)
        );
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateMedicalRecordRequest request, long workspaceId, long patientId,
        long appointmentId, CancellationToken cancellationToken)
    {
        var command = new CreateMedicalRecordCommand
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
    public async Task<IActionResult> Update(long id, UpdateMedicalRecordRequest request, long workspaceId,
        long patientId, long appointmentId, CancellationToken cancellationToken)
    {
        if (id != request.Id) return BadRequest();
        
        var command = new UpdateMedicalRecordCommand
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
        var command = new DeleteMedicalRecordCommand
            { Id = id, WorkspaceId = workspaceId, PatientId = patientId, AppointmentId = appointmentId };
        var result = await mediator.Send(command, cancellationToken);

        return result.Match<IActionResult>(
            result => NoContent(),
            errors => Problem(errors)
        );
    }
}