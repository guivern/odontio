using Odontio.Application.Common.Interfaces;
using Odontio.Application.Workspaces.Common;

namespace Odontio.Application.Workspaces.Commands.UpdateWorkspace;

public class UpdateWorkspaceHandler(IApplicationDbContext context, IMapper mapper)
    : IRequestHandler<UpdateWorkspaceCommand, ErrorOr<UpsertWorkspaceResult>>
{
    public async Task<ErrorOr<UpsertWorkspaceResult>> Handle(UpdateWorkspaceCommand request,
        CancellationToken cancellationToken)
    {
        var entity = await context.Workspaces.FindAsync(request.Id);

        if (entity == null)
        {
            return Error.NotFound(description: "Workspace was not found.");
        }

        entity = mapper.Map(request, entity);

        context.Workspaces.Entry(entity).State = EntityState.Modified;

        try
        {
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException)
        {
            return Error.Conflict(description: "The workspace was modified by another user.");
        }

        var dto = mapper.Map<UpsertWorkspaceResult>(entity);

        return dto;
    }
}