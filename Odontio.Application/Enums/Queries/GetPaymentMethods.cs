using Odontio.Application.Common.Interfaces;
using Odontio.Application.Enums.Common;
using Odontio.Domain.Enums;

namespace Odontio.Application.Enums.Queries;

public class GetPaymentMethodsQuery : IRequest<List<EnumValue>>
{
}

public class GetPaymentMethodsQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetPaymentMethodsQuery, List<EnumValue>>
{
    public Task<List<EnumValue>> Handle(GetPaymentMethodsQuery request, CancellationToken cancellationToken)
    {
        var result = new List<EnumValue>
        {
            new(nameof(PaymentMethod.Cash), "Efectivo"),
            new(nameof(PaymentMethod.Transfer), "Transferencia"),
            new(nameof(PaymentMethod.Other), "Otro")
        };

        return Task.FromResult(result);
    }
}