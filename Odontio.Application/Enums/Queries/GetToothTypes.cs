using Odontio.Application.Enums.Common;
using Odontio.Domain.Enums;

namespace Odontio.Application.Enums.Queries;

public class GetToothTypes : IRequest<List<string>>
{
}

public class GetToothTypesHandler : IRequestHandler<GetToothTypes, List<string>>
{
    public Task<List<string>> Handle(GetToothTypes request, CancellationToken cancellationToken)
    {
        var result = Enum.GetNames(typeof(ToothType)).ToList();

        return Task.FromResult(result);
    }
}