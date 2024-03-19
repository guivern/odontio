using Odontio.Application.Common.Interfaces;

namespace Odontio.Application.Workspaces.Commands.DeleteWorkspace;

public class DeleteWorkspaceHandler(IApplicationDbContext context)
    : IRequestHandler<DeleteWorkspaceCommand, ErrorOr<Unit>>
{
    public async Task<ErrorOr<Unit>> Handle(DeleteWorkspaceCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.Workspaces.FindAsync(request.Id);

        if (entity == null)
        {
            return Error.NotFound(description: "Workspace not found.");
        }

        context.Workspaces.Remove(entity);

        try
        {
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException e) // TODO: REPLICAR EN LOS OTROS ENTITIES QUE TIENEN DEPENDENCIAS
        {
            return Error.Conflict(description: "Can not delete workspace due to existing dependencies.");
        }

        return Unit.Value;
    }
}