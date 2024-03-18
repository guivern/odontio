using Odontio.Application.Enums.Common;
using Odontio.Domain.Enums;

namespace Odontio.Application.Enums.Queries;

public class GetOdontogramTypes : IRequest<List<EnumValue>>
{
}

public class GetOdontogramTypesHandler : IRequestHandler<GetOdontogramTypes, List<EnumValue>>
{
    public Task<List<EnumValue>> Handle(GetOdontogramTypes request, CancellationToken cancellationToken)
    {
        var result = new List<EnumValue>
        {
            new(nameof(Odontogram.Child), "Niño"),
            new(nameof(Odontogram.Adult), "Adulto")
        };

        return Task.FromResult(result);
    }
}