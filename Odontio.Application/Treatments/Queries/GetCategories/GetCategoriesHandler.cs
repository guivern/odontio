using Odontio.Application.Common.Helpers;
using Odontio.Application.Common.Interfaces;
using Odontio.Domain.Entities;

namespace Odontio.Application.Treatments.Queries.GetCategories;

public class GetCategoriesHandler(IApplicationDbContext context)
    : IRequestHandler<GetCategoriesQuery, ErrorOr<PagedList<GetCategoriesResult>>>
{
    public async Task<ErrorOr<PagedList<GetCategoriesResult>>> Handle(GetCategoriesQuery request,
        CancellationToken cancellationToken)
    {
        var query = context.Categories
            .AsNoTracking()
            .ProjectToType<GetCategoriesResult>()
            .AsQueryable();

        if (!string.IsNullOrEmpty(request.Filter))
        {
            query = query.Filter(request.Filter, new List<string>
            {
                nameof(Category.Name),
            });
        }

        if (request.OrderBy != null && request.OrderBy.Count != 0)
        {
            query = query.OrderBy(request.OrderBy);
        }

        return await PagedList<GetCategoriesResult>.CreateAsync(query, request.Page, request.PageSize, cancellationToken);
    }
}