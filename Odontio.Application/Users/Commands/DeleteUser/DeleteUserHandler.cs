using Odontio.Application.Common.Interfaces;

namespace Odontio.Application.Users.Commands.DeleteUser;

public class DeleteUserHandler(IApplicationDbContext context) : IRequestHandler<DeleteUserCommand, ErrorOr<Unit>>
{
    public async Task<ErrorOr<Unit>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await context.Users.FindAsync(request.Id);
        
        if (user == null)
        {
            return Error.NotFound();
        }
        
        context.Users.Remove(user);

        try
        {
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException e)
        {
            return Error.Conflict(description: "The User can not be deleted due to existing dependencies.");
        }
        
        return Unit.Value;
    }
}