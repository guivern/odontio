using Odontio.Application.Theeth.Query.GetTeeth;

namespace Odontio.API.Controllers;

public class TheethController(IMediator mediator) : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetTheethQuery request)
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