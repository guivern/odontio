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
}