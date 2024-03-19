using Odontio.Application.Common.Interfaces;
using Odontio.Application.Workspaces.Common;

namespace Odontio.Application.Workspaces.Queries.GetWorkspaceById;

public class GetWorkspaceByIdHandler (IApplicationDbContext context, IMapper mapper)
    : IRequestHandler<GetWorkspaceByIdQuery, ErrorOr<UpsertWorkspaceResult>>
{
    public async Task<ErrorOr<UpsertWorkspaceResult>> Handle(GetWorkspaceByIdQuery request,
        CancellationToken cancellationToken)
    {
        var entity = await context.Workspaces.FindAsync(request.Id);

        if (entity == null)
        {
            return Error.NotFound(description: "Workspace not found.");
        }

        var dto = mapper.Map<UpsertWorkspaceResult>(entity);

        return dto;
    }
}