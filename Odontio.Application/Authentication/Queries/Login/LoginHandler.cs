using System.Net;
using ErrorOr;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Odontio.Application.Authentication.Common;
using Odontio.Application.Common.Interfaces;

namespace Odontio.Application.Authentication.Queries.Login;

public class LoginHandler(IApplicationDbContext context, IAuthService authService, IMapper mapper)
    : IRequestHandler<LoginQuery, ErrorOr<AuthenticationResult>>
{
    public async Task<ErrorOr<AuthenticationResult>> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        var user = await context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Username == request.Username, cancellationToken: cancellationToken);

        if (user == null)
            return Error.Custom(code: "INVALID_CREDENTIALS", description: "Invalid username or password",
                type: (int)HttpStatusCode.Unauthorized);

        var isValidPassword = authService.VerifyPassword(request.Password, user.PasswordHash, user.PasswordSalt);

        if (!isValidPassword)
            return Error.Custom(code: "INVALID_CREDENTIALS", description: "Invalid username or password",
                type: (int)HttpStatusCode.Unauthorized);

        var token = authService.GenerateJwtToken(user);
        var result = mapper.Map<AuthenticationResult>(user);
        // set token
        result = result with { Token = token };

        return result;
    }
}