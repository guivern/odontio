using Odontio.Application.Common.Helpers;
using Odontio.Application.Common.Interfaces;
using Odontio.Application.Diseases.Common;
using Odontio.Domain.Entities;

namespace Odontio.Application.Diseases.Queries.GetDiseases;

public class GetDiseasesHandler(IApplicationDbContext context)
    : IRequestHandler<GetDiseasesQuery, ErrorOr<PagedList<UpsertDiseaseResult>>>
{
    public async Task<ErrorOr<PagedList<UpsertDiseaseResult>>> Handle(GetDiseasesQuery request, CancellationToken cancellationToken)
    {
        var query = context.Diseases
            .AsNoTracking()
            .Where(x => x.WorkspaceId == request.WorkspaceId)
            .ProjectToType<UpsertDiseaseResult>()
            .AsQueryable();


        if (!string.IsNullOrEmpty(request.Filter))
        {
            query = query.Filter(request.Filter, new List<string>
            {
                nameof(Disease.Name),
            });
        }

        if (request.OrderBy != null && request.OrderBy.Count != 0)
        {
            query = query.OrderBy(request.OrderBy);
        }

        return await PagedList<UpsertDiseaseResult>.CreateAsync(query, request.Page, request.PageSize);
    }
}