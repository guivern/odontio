using MapsterMapper;
using Odontio.API.Contracts.MedicalConditions;
using Odontio.Application.MedicalConditions.Common.AddMedicalConditions;
using Odontio.Application.MedicalConditions.Common.DeleteMedicalCondition;
using Odontio.Application.MedicalConditions.Queries.GetMedicalConditions;
using Odontio.Application.MedicalConditions.UpdateMedicalCondition;

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

    [HttpPost]
    public async Task<IActionResult> AddRange(long workspaceId, long patientId,
        List<AddMedicalConditionRequest> request,
        CancellationToken cancellationToken)
    {
        var command = new AddMedicalConditionsCommand
        {
            PatientId = patientId,
            WorkspaceId = workspaceId,
            MedicalConditions = mapper.Map<List<AddMedicalConditionDto>>(request)
        };

        var result = await mediator.Send(command, cancellationToken);

        return result.Match<IActionResult>(
            result => Ok(result),
            errors => Problem(errors)
        );
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> Update(long workspaceId, long patientId, long id,
        [FromBody] UpdateMedicalConditionRequest request,
        CancellationToken cancellationToken)
    {
        var command = mapper.Map<UpdateMedicalConditionCommand>(request);

        command.Id = id;
        command.PatientId = patientId;
        command.WorkspaceId = workspaceId;

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