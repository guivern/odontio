using Odontio.Application.Enums.Common;
using Odontio.Domain.Enums;

namespace Odontio.Application.Enums.Queries;

public class GetBudgetStatus : IRequest<List<EnumValue>>
{
}

public class GetBudgetStatusHandler : IRequestHandler<GetBudgetStatus, List<EnumValue>>
{
    public Task<List<EnumValue>> Handle(GetBudgetStatus request, CancellationToken cancellationToken)
    {
        var result = new List<EnumValue>
        {
            new(nameof(BudgetStatus.Pending), "Pendiente"),
            new(nameof(BudgetStatus.Approved), "Aceptado"),
            new(nameof(BudgetStatus.Rejected), "Rechazado")
        };

        return Task.FromResult(result);
    }
}