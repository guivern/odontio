using Odontio.API.Contracts.Budgets;
using Odontio.Application.Budgets.Commands.CreateBudget;
using Odontio.Application.Budgets.Commands.DeleteBudget;
using Odontio.Application.Budgets.Commands.UpdateBudget;
using Odontio.Application.Budgets.Queries.GetBudgetById;
using Odontio.Application.Budgets.Queries.GetBudgetsByPatient;

namespace Odontio.API.Controllers;

[Route("api/v1/Workspaces/{workspaceId}/patients/{patientId}/[controller]")]
public class BudgetsController(IMediator mediator, IMapper mapper) : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetBudgetsByPatient(long workspaceId, long patientId,
        PaginationQueryParams pagination, CancellationToken cancellationToken)
    {
        var query = new GetBudgetsByPatientQuery
        {
            PatientId = patientId,
            WorkspaceId = workspaceId,
            Page = pagination.Page,
            PageSize = pagination.PageSize,
            Filter = pagination.Filter,
            OrderBy = pagination.OrderBy
        };
        
        var result = await mediator.Send(query, cancellationToken);

        Response.AddPaginationHeader(result.PageNumber, result.PageSize, result.TotalCount, result.TotalPages);
        return Ok(result);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetBudgetById(long id, long workspaceId, long patientId, CancellationToken cancellationToken)
    {
        var query = new GetBudgetByIdQuery { Id = id, WorkspaceId = workspaceId, PatientId = patientId };
        var result = await mediator.Send(query, cancellationToken);

        return result.Match<IActionResult>(
            result => Ok(result),
            errors => Problem(errors)
        );
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateBudget(long workspaceId, long patientId, CreateBudgetRequest request,
        CancellationToken cancellationToken)
    {
        var command = mapper.Map<CreateBudgetCommand>(request);
        command.WorkspaceId = workspaceId;
        command.PatientId = patientId;

        var result = await mediator.Send(command, cancellationToken);

        return result.Match<IActionResult>(
            result => CreatedAtAction(nameof(GetBudgetById),
                new GetBudgetByIdQuery { Id = result.Id, WorkspaceId = workspaceId, PatientId = patientId }, result),
            errors => Problem(errors)
        );
    }
    
    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateBudget(long id, long workspaceId, long patientId, UpdateBudgetRequest request,
        CancellationToken cancellationToken)
    {
        var command = mapper.Map<UpdateBudgetCommand>(request);
        
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
    public async Task<IActionResult> DeleteBudget(long id, long workspaceId, long patientId, CancellationToken cancellationToken)
    {
        var command = new DeleteBudgetCommand { Id = id, WorkspaceId = workspaceId, PatientId = patientId };
        var result = await mediator.Send(command, cancellationToken);

        return result.Match<IActionResult>(
            result => NoContent(),
            errors => Problem(errors)
        );
    }
}