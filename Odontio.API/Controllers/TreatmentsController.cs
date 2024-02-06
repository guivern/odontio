using Odontio.Application.Treatments.Common.CreateTreatment;
using Odontio.Application.Treatments.Queries.GetTreatmentById;
using Odontio.Application.Treatments.Queries.GetTreatments;

namespace Odontio.API.Controllers;

[Route("api/v1/Workspaces/{workspaceId}/[controller]")]
public class TreatmentsController(IMediator mediator) : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(GetTreatmentsQuery request)
    {
        var result = await mediator.Send(request);

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
    public async Task<IActionResult> GetById(GetTreatmentByIdQuery request)
    {
        var result = await mediator.Send(request);

        return result.Match<IActionResult>(
            result => Ok(result),
            errors => Problem(errors)
        );
    }

    [HttpPost]
    public async Task<IActionResult> Create(long workspaceId, CreateTreatmentCommand command)
    {
        command.WorkspaceId = workspaceId;
        var result = await mediator.Send(command);

        return result.Match<IActionResult>(
            result => CreatedAtAction(nameof(GetById),
                new GetTreatmentByIdQuery { Id = result.Id, WorkspaceId = workspaceId }, result),
            errors => Problem(errors)
        );
    }
}