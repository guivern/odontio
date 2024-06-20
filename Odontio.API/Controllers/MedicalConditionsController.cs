using Odontio.API.Contracts.MedicalConditions;
using Odontio.Application.MedicalConditions.Common.UpdateMedicalConditions;
using Odontio.Application.MedicalConditions.Common.DeleteMedicalCondition;
using Odontio.Application.MedicalConditions.Queries.GetMedicalConditions;

namespace Odontio.API.Controllers;

[Route("api/v1/Workspaces/{workspaceId}/patients/{patientId}/[controller]")]
public class MedicalConditionsController(IMediator mediator, IMapper mapper) : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get(long workspaceId, long patientId, CancellationToken cancellationToken)
    {
        var query = new GetMedicalConditionsQuery
        {
            PatientId = patientId,
            WorkspaceId = workspaceId
        };

        var result = await mediator.Send(query, cancellationToken);

        return result.Match<IActionResult>(
            result => Ok(result),
            errors => Problem(errors)
        );
    }

    [HttpPatch]
    public async Task<IActionResult> Update(long workspaceId, long patientId,
        List<UpdateMedicalConditionRequest> request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateMedicalConditionsCommand
        {
            PatientId = patientId,
            WorkspaceId = workspaceId,
            MedicalConditions = mapper.Map<List<MedicalConditionDto>>(request)
        };

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
        var command = new DeleteMedicalConditionCommand
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