using Odontio.API.Contracts.MedicalConditionQuestions;
using Odontio.Application.MedicalConditionQuestions.Commands.CreateMedicalConditionQuestion;
using Odontio.Application.MedicalConditionQuestions.Commands.DeleteMedicalConditionQuestion;
using Odontio.Application.MedicalConditionQuestions.Commands.UpdateMedicalConditionQuestion;
using Odontio.Application.MedicalConditionQuestions.Queries.GetMedicalConditionQuestionById;
using Odontio.Application.MedicalConditionQuestions.Queries.GetMedicalConditionQuestions;

namespace Odontio.API.Controllers;

[Route("api/v1/Workspaces/{workspaceId}/[controller]")]
public class MedicalConditionQuestionsController(IMediator mediator, IMapper mapper) : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(long workspaceId, PaginationQueryParams pagination,
        CancellationToken cancellationToken)
    {
        var request = new GetMedicalConditionQuestionsQuery
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
        var request = new GetMedicalConditionQuestionByIdQuery { Id = id, WorkspaceId = workspaceId };
        var result = await mediator.Send(request, cancellationToken);

        return result.Match<IActionResult>(
            result => Ok(result),
            errors => Problem(errors)
        );
    }

    [HttpPost]
    public async Task<IActionResult> Create(long workspaceId, CreateMedicalConditionQuestionRequest request,
        CancellationToken cancellationToken)
    {
        var command = mapper.Map<CreateMedicalConditionQuestionCommand>(request);

        command.WorkspaceId = workspaceId;

        var result = await mediator.Send(command, cancellationToken);

        return result.Match<IActionResult>(
            result => CreatedAtAction(nameof(GetById), new { id = result.Id, workspaceId }, result),
            errors => Problem(errors)
        );
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> Update(long id, long workspaceId, UpdateMedicalConditionQuestionRequest request,
        CancellationToken cancellationToken)
    {
        var command = mapper.Map<UpdateMedicalConditionQuestionCommand>(request);

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
        var command = new DeleteMedicalConditionQuestionCommand { Id = id, WorkspaceId = workspaceId };

        var result = await mediator.Send(command, cancellationToken);

        return result.Match<IActionResult>(
            _ => NoContent(),
            errors => Problem(errors)
        );
    }
}