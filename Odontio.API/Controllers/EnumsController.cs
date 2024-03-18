using Odontio.Application.Enums.Queries;
using Odontio.Domain.Enums;

namespace Odontio.API.Controllers;

[Route("api/v1/[controller]")]
public class EnumsController(IMediator mediator, IMapper mapper) : ApiControllerBase
{
    [HttpGet("PaymentMethods")]
    public async Task<IActionResult> GetPaymentMethods(CancellationToken cancellationToken)
    {
        var query = new GetPaymentMethodsQuery();
        var result = await mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpGet("ReceiptTypes")]
    public async Task<IActionResult> GetReceiptTypes(CancellationToken cancellationToken)
    {
        var query = new GetReceiptTypes();
        var result = await mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpGet("TreatmentStatus")]
    public async Task<IActionResult> GetTreatmentStatus(CancellationToken cancellationToken)
    {
        var query = new GetTreatmentStatus();
        var result = await mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpGet("BudgetStatus")]
    public async Task<IActionResult> GetBudgetStatus(CancellationToken cancellationToken)
    {
        var query = new GetBudgetStatus();
        var result = await mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpGet("MaritalStatus/{gender}")]
    public async Task<IActionResult> GetMaritalStatus(CancellationToken cancellationToken, string gender)
    {
        var query = new GetMaritalStatus { Gender = gender };

        var result = await mediator.Send(query, cancellationToken);

        return result.Match<IActionResult>(
            result => Ok(result),
            errors => Problem(errors)
        );
    }

    [HttpGet("Genders")]
    public async Task<IActionResult> GetGenders(CancellationToken cancellationToken)
    {
        var query = new GetGenders();
        var result = await mediator.Send(query, cancellationToken);
        return Ok(result);
    }
    
    [HttpGet("OdontogramTypes")]
    public async Task<IActionResult> GetToothTypes(CancellationToken cancellationToken)
    {
        var query = new GetOdontogramTypes();
        var result = await mediator.Send(query, cancellationToken);
        return Ok(result);
    }
    
    [HttpGet("ToothTypes")]
    public async Task<IActionResult> GetOdontogramTypes(CancellationToken cancellationToken)
    {
        var query = new GetToothTypes();
        var result = await mediator.Send(query, cancellationToken);
        return Ok(result);
    }
}