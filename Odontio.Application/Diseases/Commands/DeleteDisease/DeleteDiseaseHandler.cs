using Odontio.Application.Common.Interfaces;

namespace Odontio.Application.Diseases.Commands.DeleteDisease;

public class DeleteDiseaseHandler(IApplicationDbContext context) : IRequestHandler<DeleteDiseaseCommand, ErrorOr<Unit>>
{
    public async Task<ErrorOr<Unit>> Handle(DeleteDiseaseCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.Diseases
            .Where(x => x.Id == request.Id)
            .Where(x => x.WorkspaceId == request.WorkspaceId)
            .FirstOrDefaultAsync(cancellationToken);

        if (entity == null)
        {
            return Error.NotFound(description: "Disease not found");
        }

        context.Diseases.Remove(entity);

        try
        {
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException e)
        {
            return Error.Conflict(description: "Can not delete disease due to existing dependencies.");
        }

        return Unit.Value;
    }
}