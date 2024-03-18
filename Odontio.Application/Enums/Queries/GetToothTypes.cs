using Odontio.Application.Enums.Common;
using Odontio.Domain.Enums;

namespace Odontio.Application.Enums.Queries;

public class GetToothTypes : IRequest<List<EnumValue>>
{
}

public class GetToothTypesHandler : IRequestHandler<GetToothTypes, List<EnumValue>>
{
    public Task<List<EnumValue>> Handle(GetToothTypes request, CancellationToken cancellationToken)
    {
        var result = new List<EnumValue>
        {
            new(nameof(ToothType.Incisive), "Incisivo"),
            new(nameof(ToothType.Canine), "Canino"),
            new(nameof(ToothType.Premolar), "Premolar"),
            new(nameof(ToothType.Molar), "Molar")
        };

        return Task.FromResult(result);
    }
}