using Odontio.Application.Teeth.Query.GetTeeth;

namespace Odontio.API.Controllers;

public class TeethController(IMediator mediator) : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetTeethQuery request)
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
}