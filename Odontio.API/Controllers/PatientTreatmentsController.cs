using Odontio.API.Contracts.Budgets;
using Odontio.Application.PatientTreatments.Commands.CreatePatientTreatment;
using Odontio.Application.PatientTreatments.Commands.DeletePatientTreatment;
using Odontio.Application.PatientTreatments.Commands.UpdatePatientTreatment;
using Odontio.Application.PatientTreatments.Queries.GetPatientTreatmentById;

namespace Odontio.API.Controllers;

[Route("api/v1/Workspaces/{workspaceId}/patients/{patientId}/budgets/{budgetId}/[controller]")]
public class PatientTreatmentsController(IMediator mediator, IMapper mapper) : ApiControllerBase
{
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(long id, long workspaceId, long patientId, long budgetId,
        CancellationToken cancellationToken)
    {
        var query = new GetPatientTreatmentByIdQuery
            { Id = id, WorkspaceId = workspaceId, PatientId = patientId, BudgetId = budgetId };
        var result = await mediator.Send(query, cancellationToken);

        return result.Match<IActionResult>(
            result => Ok(result),
            errors => Problem(errors)
        );
    }

    [HttpPost]
    public async Task<IActionResult> Create(long workspaceId, long patientId, long budgetId,
        CreatePatientTreatmentRequest request,
        CancellationToken cancellationToken)
    {
        var command = mapper.Map<CreatePatientTreatmentCommand>(request);
        command.WorkspaceId = workspaceId;
        command.PatientId = patientId;
        command.BudgetId = budgetId;

        var result = await mediator.Send(command, cancellationToken);

        return result.Match<IActionResult>(
            result => CreatedAtAction(nameof(GetById),
                new GetPatientTreatmentByIdQuery
                    { Id = result.Id, WorkspaceId = workspaceId, PatientId = patientId, BudgetId = budgetId }, result),
            errors => Problem(errors)
        );
    }
    
    [HttpPatch("{id}")]
    public async Task<IActionResult> Update(long id, long workspaceId, long patientId, long budgetId,
        UpdatePatientTreatmentRequest request, CancellationToken cancellationToken)
    {
        var command = mapper.Map<UpdatePatientTreatmentCommand>(request);
        command.Id = id;
        command.WorkspaceId = workspaceId;
        command.PatientId = patientId;
        command.BudgetId = budgetId;

        var result = await mediator.Send(command, cancellationToken);

        return result.Match<IActionResult>(
            result => Ok(result),
            errors => Problem(errors)
        );
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id, long workspaceId, long patientId, long budgetId,
        CancellationToken cancellationToken)
    {
        var command = new DeletePatientTreatmentCommand
        {
            Id = id,
            WorkspaceId = workspaceId,
            PatientId = patientId,
            BudgetId = budgetId
        };

        var result = await mediator.Send(command, cancellationToken);

        return result.Match<IActionResult>(
            result => NoContent(),
            errors => Problem(errors)
        );
    }
}