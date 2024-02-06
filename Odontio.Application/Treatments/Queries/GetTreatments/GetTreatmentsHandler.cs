using Odontio.Application.Common.Helpers;
using Odontio.Application.Common.Interfaces;

namespace Odontio.Application.Treatments.Queries.GetTreatments;

public class GetTreatmentsHandler(IApplicationDbContext context)
    : IRequestHandler<GetTreatmentsQuery, ErrorOr<PagedList<GetTreatmentsResult>>>
{
    public async Task<ErrorOr<PagedList<GetTreatmentsResult>>> Handle(GetTreatmentsQuery request,
        CancellationToken cancellationToken)
    {
        var workspaceExists = await context.Workspaces.AsNoTracking()
            .AnyAsync(x => x.Id == request.WorkspaceId, cancellationToken);
        
        if (!workspaceExists)
        {
            return Error.NotFound(description: "Workspace not found");
        }
        
        var query = context.Treatments
            .AsNoTracking()
            .Include(x => x.Category)
            .Where(x => x.WorkspaceId == request.WorkspaceId)
            .ProjectToType<GetTreatmentsResult>()
            .AsQueryable();

        if (!string.IsNullOrEmpty(request.Filter))
        {
            query = query.Filter(request.Filter, new List<string>
            {
                nameof(GetTreatmentsResult.Name),
                nameof(GetTreatmentsResult.CategoryName),
            });
        }

        if (request.OrderBy != null && request.OrderBy.Count != 0)
        {
            query = query.OrderBy(request.OrderBy);
        }

        return await PagedList<GetTreatmentsResult>.CreateAsync(query, request.Page, request.PageSize);
    }
}