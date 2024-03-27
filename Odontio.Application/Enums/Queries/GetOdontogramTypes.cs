using Odontio.Application.Enums.Common;
using Odontio.Domain.Enums;

namespace Odontio.Application.Enums.Queries;

public class GetOdontogramTypes : IRequest<List<string>>
{
}

public class GetOdontogramTypesHandler : IRequestHandler<GetOdontogramTypes, List<string>>
{
    public Task<List<string>> Handle(GetOdontogramTypes request, CancellationToken cancellationToken)
    {
        var result = Enum.GetNames(typeof(OdontogramType)).ToList();
        return Task.FromResult(result);
    }
}