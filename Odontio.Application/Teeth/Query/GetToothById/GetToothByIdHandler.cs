using Odontio.Application.Common.Interfaces;
using Odontio.Application.Teeth.Query.Common;

namespace Odontio.Application.Teeth.Query.GetToothById;

public class GetToothByIdHandler(IApplicationDbContext context, IMapper mapper)
    : IRequestHandler<GetToothByIdQuery, ErrorOr<GetToothResult>>
{
    public async Task<ErrorOr<GetToothResult>> Handle(GetToothByIdQuery request,
        CancellationToken cancellationToken)
    {
        var entity = await context.Teeth.FindAsync(request.Id);

        if (entity == null)
        {
            return Error.NotFound(nameof(Teeth), "Tooth not found.");
        }

        var dto = mapper.Map<GetToothResult>(entity);

        return dto;
    }
}