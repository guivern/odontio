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
        catch (DbUpdateException e)
        {
            return Error.Conflict(description: "No fue posible eliminar el workspace debido a que tiene pacientes asociados. Primero elimine todos los pacientes del workspace.");
        }

        return Unit.Value;
    }
}