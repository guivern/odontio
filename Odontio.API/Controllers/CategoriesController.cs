using Odontio.Application.Categories.Queries.GetCategories;

namespace Odontio.API.Controllers;

public class CategoriesController(IMediator mediator) : ApiControllerBase
{
    [HttpGet]
    // [Authorize(Policy = nameof(Roles.Administrator))]
    public async Task<IActionResult> GetAll([FromQuery] GetCategoriesQuery request, CancellationToken cancellationToken)
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
}