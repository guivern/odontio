using Odontio.Application.Enums.Common;
using Odontio.Domain.Enums;

namespace Odontio.Application.Enums.Queries;

public class GetReceiptTypes : IRequest<List<string>>
{ }

public class GetReceiptTypesHandler : IRequestHandler<GetReceiptTypes, List<string>>
{
    public Task<List<string>> Handle(GetReceiptTypes request, CancellationToken cancellationToken)
    {
        var result = Enum.GetNames(typeof(ReceiptType)).ToList();

        return Task.FromResult(result);
    }
}