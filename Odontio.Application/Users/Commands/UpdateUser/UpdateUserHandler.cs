using Odontio.Application.Common.Interfaces;
using Odontio.Application.Users.Common;

namespace Odontio.Application.Users.Commands.UpdateUser;

public class UpdateUserHandler (IApplicationDbContext context, IMapper mapper, IAuthService authService) : IRequestHandler<UpdateUserCommand, ErrorOr<UpsertUserResult>>
{
    public async Task<ErrorOr<UpsertUserResult>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.Users.FindAsync(request.Id);

        if (entity == null || !entity.IsActive)
        {
            return Error.NotFound();
        }

        entity = mapper.Map(request, entity);

        if (!string.IsNullOrEmpty(request.Password))
        {
            entity.PasswordSalt = authService.GeneratePasswordSalt();
            entity.PasswordHash = authService.GeneratePasswordHash(request.Password, entity.PasswordSalt);
        }

        context.Users.Entry(entity).State = EntityState.Modified;

        try
        {
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException)
        {
            return Error.Conflict(description: "The User was modified by another user");
        }

        var result = mapper.Map<UpsertUserResult>(entity);
        
        result.Token = authService.GenerateJwtToken(entity);

        return result;
    }
}