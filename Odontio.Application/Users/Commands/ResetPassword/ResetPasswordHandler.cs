using Odontio.Application.Common.Interfaces;

namespace Odontio.Application.Users.Commands.ResetPassword;

public class ResetPasswordHandler(IApplicationDbContext context, IAuthService authService): IRequestHandler<ResetPasswordCommand, ErrorOr<Unit>>
{
    public async Task<ErrorOr<Unit>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await context.Users
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (user == null)
        {
            return Error.NotFound(description: "User not found");
        }

        user.PasswordSalt = authService.GeneratePasswordSalt();
        user.PasswordHash = authService.GeneratePasswordHash(request.Password, user.PasswordSalt);

        context.Users.Entry(user).State = EntityState.Modified;

        try
        {
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException)
        {
            return Error.Conflict(description: "The User was modified by another user");
        }

        return Unit.Value;
    }
}