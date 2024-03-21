using Odontio.Application.Authentication.Common;
using Odontio.Application.Common.Interfaces;

namespace Odontio.Application.Authentication.Commands.ChangePassword;

public class ChangePasswordHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
    : IRequestHandler<ChangePasswordCommand, ErrorOr<AuthenticationResult>>
{
    public async Task<ErrorOr<AuthenticationResult>> Handle(ChangePasswordCommand request,
        CancellationToken cancellationToken)
    {
        var userId = authService.GetCurrentUserId();
        var user = await context.Users
            .FirstOrDefaultAsync(x => x.Username == request.Username && x.IsActive, cancellationToken);
        
        if (user == null || user.Id != userId)
        {
            return Error.Unauthorized();
        }

        var isValidPassword = authService.VerifyPassword(request.OldPassword, user.PasswordHash, user.PasswordSalt);

        if (!isValidPassword)
        {
            return Error.Unauthorized();
        }

        user.PasswordSalt = authService.GeneratePasswordSalt();
        user.PasswordHash = authService.GeneratePasswordHash(request.NewPassword, user.PasswordSalt);

        context.Users.Entry(user).State = EntityState.Modified;

        try
        {
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException)
        {
            return Error.Conflict(description: "The User was modified by another user");
        }

        var result = mapper.Map<AuthenticationResult>(user);

        result = result with { Token = authService.GenerateJwtToken(user) };

        return result;
    }
}