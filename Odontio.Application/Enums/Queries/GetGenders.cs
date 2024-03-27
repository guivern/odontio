using Odontio.Application.Enums.Common;
using Odontio.Domain.Enums;

namespace Odontio.Application.Enums.Queries;

public class GetGenders : IRequest<List<string>>
{
}

public class GetGendersHandler : IRequestHandler<GetGenders, List<string>>
{
    public Task<List<string>> Handle(GetGenders request, CancellationToken cancellationToken)
    {
        var result = Enum.GetNames(typeof(Gender)).ToList();

        return Task.FromResult(result);
    }
}