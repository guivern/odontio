using MapsterMapper;
using Odontio.API.Contracts.Treatments;
using Odontio.Application.Treatments.Commands.CreateTreatment;
using Odontio.Application.Treatments.Commands.DeleteTreatment;
using Odontio.Application.Treatments.Commands.UpdateTreatment;
using Odontio.Application.Treatments.Queries.GetCategories;
using Odontio.Application.Treatments.Queries.GetTreatmentById;
using Odontio.Application.Treatments.Queries.GetTreatments;

namespace Odontio.API.Controllers;

[Route("api/v1/Workspaces/{workspaceId}/[controller]")]
public class TreatmentsController(IMediator mediator, IMapper mapper) : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(long workspaceId, PaginationQueryParams pagination,
        CancellationToken cancellationToken)
    {
        var request = new GetTreatmentsQuery
        {
            WorkspaceId = workspaceId,
            Page = pagination.Page,
            PageSize = pagination.PageSize,
            Filter = pagination.Filter,
            OrderBy = pagination.OrderBy
        };

        var result = await mediator.Send(request, cancellationToken);

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
    public async Task<IActionResult> GetById(long id, long workspaceId, CancellationToken cancellationToken)
    {
        var request = new GetTreatmentByIdQuery { Id = id, WorkspaceId = workspaceId };
        var result = await mediator.Send(request, cancellationToken);

        return result.Match<IActionResult>(
            result => Ok(result),
            errors => Problem(errors)
        );
    }
    
    [HttpGet("categories")]
    public async Task<IActionResult> GetTreatmentCategories([FromQuery] GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(request, cancellationToken);
        
        return result.Match<IActionResult>(
            result =>
            {
                Response.AddPaginationHeader(result.PageNumber, result.PageSize, result.TotalCount, result.TotalPages);
                return Ok(result);
            },
            errors => Problem(errors)
        );
    }

    [HttpPost]
    public async Task<IActionResult> Create(long workspaceId, CreateTreatmentRequest request,
        CancellationToken cancellationToken)
    {
        var command = mapper.Map<CreateTreatmentCommand>(request);
        command.WorkspaceId = workspaceId;

        var result = await mediator.Send(command, cancellationToken);

        return result.Match<IActionResult>(
            result => CreatedAtAction(nameof(GetById),
                new GetTreatmentByIdQuery { Id = result.Id, WorkspaceId = workspaceId }, result),
            errors => Problem(errors)
        );
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> Update(long workspaceId, long id, UpdateTreatmentRequest request,
        CancellationToken cancellationToken)
    {
        if (id != request.Id)
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Invalid request",
                Detail = "The id in the request body does not match the id in the route"
            });
        }

        var command = mapper.Map<UpdateTreatmentCommand>(request);
        command.WorkspaceId = workspaceId;

        var result = await mediator.Send(command, cancellationToken);

        return result.Match<IActionResult>(
            result => Ok(result),
            errors => Problem(errors)
        );
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long workspaceId, long id, CancellationToken cancellationToken)
    {
        var command = new DeleteTreatmentCommand { Id = id, WorkspaceId = workspaceId };
        var result = await mediator.Send(command, cancellationToken);

        return result.Match<IActionResult>(
            result => NoContent(),
            errors => Problem(errors)
        );
    }
}