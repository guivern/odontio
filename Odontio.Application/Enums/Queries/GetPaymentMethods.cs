using Odontio.Application.Common.Interfaces;
using Odontio.Application.Enums.Common;
using Odontio.Domain.Enums;

namespace Odontio.Application.Enums.Queries;

public class GetPaymentMethodsQuery : IRequest<List<string>>
{
}

public class GetPaymentMethodsQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetPaymentMethodsQuery, List<string>>
{
    public Task<List<string>> Handle(GetPaymentMethodsQuery request, CancellationToken cancellationToken)
    {
        var result = Enum.GetNames(typeof(PaymentMethod)).ToList();

        return Task.FromResult(result);
    }
}