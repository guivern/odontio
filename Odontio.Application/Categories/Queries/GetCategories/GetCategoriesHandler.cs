using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Odontio.Application.Categories.Common;
using Odontio.Application.Common.Helpers;
using Odontio.Application.Common.Interfaces;
using Odontio.Domain.Entities;
using ErrorOr;

namespace Odontio.Application.Categories.Queries.GetCategories;

public class GetCategoriesHandler(IApplicationDbContext context, IMapper mapper)
    : IRequestHandler<GetCategoriesQuery, ErrorOr<PagedList<GetCategoryResult>>>
{
    public async Task<ErrorOr<PagedList<GetCategoryResult>>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        var query = context.Categories
            .AsNoTracking()
            .ProjectToType<GetCategoryResult>()
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

        return await PagedList<GetCategoryResult>.CreateAsync(query, request.Page, request.PageSize);
    }
}