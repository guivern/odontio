using Odontio.Application.Common.Interfaces;

namespace Odontio.Application.Users.Commands.DeleteUser;

public class DeleteUserHandler(IApplicationDbContext context,  IAuthService authService) : IRequestHandler<DeleteUserCommand, ErrorOr<Unit>>
{
    public async Task<ErrorOr<Unit>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await context.Users.FindAsync(request.Id);
        
        if (user == null)
        {
            return Error.NotFound();
        }
        
        // can not delete the current user
        var userId = authService.GetCurrentUserId();
        if (user.Id == userId)
        {
            return Error.Conflict(description: "The User can not be deleted because it is the current user.");
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