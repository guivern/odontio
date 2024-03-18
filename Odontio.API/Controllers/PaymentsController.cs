using Odontio.API.Contracts.Payments;
using Odontio.Application.Payments.Commands.CreatePayment;
using Odontio.Application.Payments.Commands.DeletePayment;
using Odontio.Application.Payments.Commands.UpdatePayment;
using Odontio.Application.Payments.Queries.GetPaymentById;

namespace Odontio.API.Controllers;

[Route("api/v1/Workspaces/{workspaceId}/patients/{patientId}/budgets/{budgetId}/[controller]")]
public class PaymentsController(IMediator mediator, IMapper mapper) : ApiControllerBase
{
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(long id, long workspaceId, long patientId, CancellationToken cancellationToken)
    {
        var query = new GetPaymentByIdQuery
            { Id = id, WorkspaceId = workspaceId, PatientId = patientId };
        var result = await mediator.Send(query, cancellationToken);

        return result.Match<IActionResult>(
            result => Ok(result),
            errors => Problem(errors)
        );
    }

    [HttpPost]
    public async Task<IActionResult> Create(long workspaceId, long patientId, long budgetId,
        CreatePaymentRequest request, CancellationToken cancellationToken)
    {
        var command = mapper.Map<CreatePaymentCommand>(request);
        command.WorkspaceId = workspaceId;
        command.PatientId = patientId;
        command.BudgetId = budgetId;

        var result = await mediator.Send(command, cancellationToken);

        return result.Match<IActionResult>(
            result => CreatedAtAction(nameof(GetById),
                new
                    { id = result.Id, workspaceId = workspaceId, patientId = patientId }, result),
            errors => Problem(errors)
        );
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> Update(long id, long workspaceId, long patientId, long budgetId,
        UpdatePaymentRequest request, CancellationToken cancellationToken)
    {
        var command = mapper.Map<UpdatePaymentCommand>(request);
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
        var command = new DeletePaymentCommand
            { Id = id, WorkspaceId = workspaceId, PatientId = patientId };
        var result = await mediator.Send(command, cancellationToken);

        return result.Match<IActionResult>(
            result => Ok(),
            errors => Problem(errors)
        );
    }
}