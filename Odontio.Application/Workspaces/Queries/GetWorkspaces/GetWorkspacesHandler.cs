using Odontio.Application.Common.Helpers;
using Odontio.Application.Common.Interfaces;
using Odontio.Application.Workspaces.Common;
using Odontio.Domain.Entities;

namespace Odontio.Application.Workspaces.Queries.GetWorkspaces;

public class GetWorkspacesHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<GetWorkspacesQuery, ErrorOr<PagedList<UpsertWorkspaceResult>>>
{
    public async Task<ErrorOr<PagedList<UpsertWorkspaceResult>>> Handle(GetWorkspacesQuery request, CancellationToken cancellationToken)
    {
        var query = context.Workspaces.AsNoTracking();

        if (!string.IsNullOrEmpty(request.Filter))
        {
            query = query.Filter(request.Filter, new List<string>
            {
                nameof(Workspace.Name),
                nameof(Workspace.Address),
                nameof(Workspace.PhoneNumber),
                nameof(Workspace.Email),
                nameof(Workspace.ContactName),
                nameof(Workspace.ContactPhoneNumber)
            });
        }
        
        if (request.OrderBy != null && request.OrderBy.Count != 0)
        {
            query = query.OrderBy(request.OrderBy);
        }
        
        var result = await PagedList<Workspace>.CreateAsync(query, request.Page, request.PageSize, cancellationToken);
        
        var dto = mapper.Map<PagedList<UpsertWorkspaceResult>>(result);
        dto.PageSize = result.PageSize;
        dto.PageNumber = result.PageNumber;
        dto.TotalCount = result.TotalCount;
        dto.TotalPages = result.TotalPages;
        
        return dto;
    }
}