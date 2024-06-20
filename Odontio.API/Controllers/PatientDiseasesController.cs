using Odontio.API.Contracts.PatientDiseases;
using Odontio.Application.PatientDiseases.Commands.UpdatePatientDiseases;
using Odontio.Application.PatientDiseases.Commands.DeletePatientDisease;
using Odontio.Application.PatientDiseases.Queries.GetPatientDiseases;

namespace Odontio.API.Controllers;

[Route("api/v1/Workspaces/{workspaceId}/Patients/{patientId}/Diseases")]
public class PatientDiseasesController(IMediator mediator, IMapper mapper) : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get(long workspaceId, long patientId, CancellationToken cancellationToken)
    {
        var query = new GetPatientDiseasesQuery()
        {
            PatientId = patientId,
            WorkspaceId = workspaceId
        };

        var result = await mediator.Send(query, cancellationToken);

        return Ok(result);
    }

    [HttpPatch]
    public async Task<IActionResult> Update(long workspaceId, long patientId, UpdatePatientDiseasesRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdatePatientDiseasesCommand
        {
            PatientId = patientId,
            WorkspaceId = workspaceId,
            DiseaseIds = request.DiseaseIds
        };

        var result = await mediator.Send(command, cancellationToken);

        return result.Match<IActionResult>(
            result => Ok(),
            errors => Problem(errors)
        );
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long workspaceId, long patientId, long id, CancellationToken cancellationToken)
    {
        var command = new DeletePatientDiseaseCommand
        {
            Id = id,
            PatientId = patientId,
            WorkspaceId = workspaceId
        };
        
        var result = await mediator.Send(command, cancellationToken);

        return result.Match<IActionResult>(
            result => Ok(result),
            errors => Problem(errors)
        );
    }
}