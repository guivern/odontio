using Odontio.Application.Common.Helpers;
using Odontio.Application.Common.Interfaces;
using Odontio.Domain.Entities;

namespace Odontio.Application.Users.Queries.GetUsers;

public class GetUsersHandler(IApplicationDbContext context, IMapper mapper)
    : IRequestHandler<GetUsersQuery, PagedList<GetUsersResult>>
{
    public async Task<PagedList<GetUsersResult>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var query = context.Users
            .AsNoTracking()
            .Include(u => u.Workspace)
            .Include(u => u.Role)
            .Where(u => u.IsActive);

        if (!string.IsNullOrEmpty(request.Filter))
        {
            query = query.Filter(request.Filter, new List<string>
            {
                $"{nameof(User.Workspace)}.{nameof(Workspace.Name)}",
                $"{nameof(User.Role)}.{nameof(Role.Name)}",
                $"{nameof(User.Username)}",
                $"{nameof(User.FirstName)}",
                $"{nameof(User.LastName)}",
                $"{nameof(User.Email)}",
            });
        }

        if (request.OrderBy != null && request.OrderBy.Count != 0)
        {
            query = query.OrderBy(request.OrderBy);
        }

        var result = await PagedList<User>.CreateAsync(query, request.Page, request.PageSize);
        var dto = mapper.Map<PagedList<GetUsersResult>>(result);

        dto.PageSize = result.PageSize;
        dto.PageNumber = result.PageNumber;
        dto.TotalCount = result.TotalCount;
        dto.TotalPages = result.TotalPages;

        return dto;
    }
}