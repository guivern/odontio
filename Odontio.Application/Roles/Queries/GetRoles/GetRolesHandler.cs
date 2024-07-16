using Odontio.Application.Common.Helpers;
using Odontio.Application.Common.Interfaces;
using Odontio.Domain.Entities;

namespace Odontio.Application.Roles.Queries.GetRoles;

public class GetRolesHandler(IApplicationDbContext context, IMapper mapper)
    : IRequestHandler<GetRolesQuery, ErrorOr<PagedList<GetRolesResult>>>
{
    public async Task<ErrorOr<PagedList<GetRolesResult>>> Handle(GetRolesQuery request,
        CancellationToken cancellationToken)
    {
        var query = context.Roles
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrEmpty(request.Filter))
        {
            query = query.Filter(request.Filter, new List<string>
            {
                $"{nameof(Role.Name)}",
            });
        }

        if (request.OrderBy != null && request.OrderBy.Count != 0)
        {
            query = query.OrderBy(request.OrderBy);
        }

        var result = await PagedList<Role>.CreateAsync(query, request.Page, request.PageSize, cancellationToken);
        var dto = mapper.Map<PagedList<GetRolesResult>>(result);

        dto.PageSize = result.PageSize;
        dto.PageNumber = result.PageNumber;
        dto.TotalCount = result.TotalCount;
        dto.TotalPages = result.TotalPages;

        return dto;
    }
}