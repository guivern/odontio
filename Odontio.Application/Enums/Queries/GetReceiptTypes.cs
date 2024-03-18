using Odontio.Application.Enums.Common;
using Odontio.Domain.Enums;

namespace Odontio.Application.Enums.Queries;

public class GetReceiptTypes : IRequest<List<EnumValue>>
{ }

public class GetReceiptTypesHandler : IRequestHandler<GetReceiptTypes, List<EnumValue>>
{
    public Task<List<EnumValue>> Handle(GetReceiptTypes request, CancellationToken cancellationToken)
    {
        var result = new List<EnumValue>
        {
            new(nameof(ReceiptType.Invoice), "Factura"),
            new(nameof(ReceiptType.Receipt), "Recibo"),
            new(nameof(ReceiptType.Other), "Otro")
        };

        return Task.FromResult(result);
    }
}