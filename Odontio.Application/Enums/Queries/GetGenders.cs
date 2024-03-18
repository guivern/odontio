using Odontio.Application.Enums.Common;
using Odontio.Domain.Enums;

namespace Odontio.Application.Enums.Queries;

public class GetGenders : IRequest<List<EnumValue>>
{
}

public class GetGendersHandler : IRequestHandler<GetGenders, List<EnumValue>>
{
    public Task<List<EnumValue>> Handle(GetGenders request, CancellationToken cancellationToken)
    {
        var result = new List<EnumValue>
        {
            new(nameof(Gender.Male), "Masculino"),
            new(nameof(Gender.Female), "Femenino")
        };

        return Task.FromResult(result);
    }
}