using Odontio.Application.Common.Interfaces;

namespace Odontio.Application.Users.Commands.ToggleActive;

public class ToggleActiveHandler(IApplicationDbContext context) : IRequestHandler<ToggleActiveCommand, ErrorOr<ToggleActiveResult>>
{
    public async Task<ErrorOr<ToggleActiveResult>> Handle(ToggleActiveCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (entity == null)
        {
            return Error.NotFound(description: "User not found");
        }

        entity.IsActive = !entity.IsActive;

        context.Users.Entry(entity).State = EntityState.Modified;

        try
        {
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException)
        {
            return Error.Conflict(description: "The User was modified by another user");
        }

        var result = new ToggleActiveResult
        {
            Id = entity.Id,
            IsActive = entity.IsActive
        };

        return result;
    }
}