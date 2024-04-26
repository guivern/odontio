using Odontio.Application.Common.Interfaces;
using Odontio.Application.Users.Common;
using Odontio.Domain.Entities;

namespace Odontio.Application.Users.Commands.CreateUser;

public class CreateUserHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
    : IRequestHandler<CreateUserCommand, ErrorOr<UpsertUserResult>>
{
    public async Task<ErrorOr<UpsertUserResult>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = mapper.Map<User>(request);

        if (string.IsNullOrEmpty(request.Password))
        {
            request.Password = authService.GenerateRandomPassword();
        }
        
        user.PasswordSalt = authService.GeneratePasswordSalt();
        user.PasswordHash = authService.GeneratePasswordHash(request.Password, user.PasswordSalt);

        context.Users.Add(user);
        await context.SaveChangesAsync(cancellationToken);

        var result = mapper.Map<UpsertUserResult>(user);
        
        result.Password = request.Password;

        return result;
    }
}