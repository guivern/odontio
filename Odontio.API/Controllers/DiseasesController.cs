using Odontio.API.Contracts.Diseases;
using Odontio.Application.Diseases.Commands.CreateDisease;
using Odontio.Application.Diseases.Commands.DeleteDisease;
using Odontio.Application.Diseases.Commands.UpdateDisease;
using Odontio.Application.Diseases.Queries.GetDiseaseById;
using Odontio.Application.Diseases.Queries.GetDiseases;

namespace Odontio.API.Controllers;

[Route("api/v1/Workspaces/{workspaceId}/[controller]")]
public class DiseasesController(IMediator mediator, IMapper mapper) : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(long workspaceId, PaginationQueryParams pagination,
        CancellationToken cancellationToken)
    {
        var request = new GetDiseasesQuery
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
        var request = new GetDiseaseByIdQuery { Id = id, WorkspaceId = workspaceId };
        var result = await mediator.Send(request, cancellationToken);

        return result.Match<IActionResult>(
            result => Ok(result),
            errors => Problem(errors)
        );
    }
    
    [HttpPost]
    public async Task<IActionResult> Create(long workspaceId, CreateDiseaseRequest request,
        CancellationToken cancellationToken)
    {
        var command = mapper.Map<CreateDiseaseCommand>(request);

        command.WorkspaceId = workspaceId;

        var result = await mediator.Send(command, cancellationToken);

        return result.Match<IActionResult>(
            result => CreatedAtAction(nameof(GetById),
                new GetDiseaseByIdQuery { Id = result.Id, WorkspaceId = command.WorkspaceId }, result),
            errors => Problem(errors)
        );
    }
    
    
    [HttpPatch("{id}")]
    public async Task<IActionResult> Update(long id, long workspaceId, UpdateDiseaseRequest request,
        CancellationToken cancellationToken)
    {
        var command = mapper.Map<UpdateDiseaseCommand>(request);

        command.Id = id;
        command.WorkspaceId = workspaceId;

        var result = await mediator.Send(command, cancellationToken);

        return result.Match<IActionResult>(
            result => Ok(result),
            errors => Problem(errors)
        );
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id, long workspaceId, CancellationToken cancellationToken)
    {
        var command = new DeleteDiseaseCommand
        {
            Id = id,
            WorkspaceId = workspaceId
        };
        
        var result = await mediator.Send(command, cancellationToken);

        return result.Match<IActionResult>(
            result => Ok(result),
            errors => Problem(errors)
        );
    }
}