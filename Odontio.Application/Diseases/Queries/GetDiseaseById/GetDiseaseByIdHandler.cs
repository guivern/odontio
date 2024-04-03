using Odontio.Application.Common.Interfaces;
using Odontio.Application.Diseases.Common;

namespace Odontio.Application.Diseases.Queries.GetDiseaseById;

public class GetDiseaseByIdHandler(IApplicationDbContext context, IMapper mapper)
    : IRequestHandler<GetDiseaseByIdQuery, ErrorOr<UpsertDiseaseResult>>
{
    public async Task<ErrorOr<UpsertDiseaseResult>> Handle(GetDiseaseByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await context.Diseases
            .AsNoTracking()
            .Where(x => x.Id == request.Id && x.WorkspaceId == request.WorkspaceId)
            .ProjectToType<UpsertDiseaseResult>()
            .FirstOrDefaultAsync(cancellationToken);

        if (entity == null)
        {
            return Error.NotFound(description: "Disease not found");
        }

        return entity;
    }
}