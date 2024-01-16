using MediatR;
using ErrorOr;
using Odontio.Application.Authentication.Common;

namespace Odontio.Application.Authentication.Queries.Login;

public record LoginQuery(string Username, string Password): IRequest<ErrorOr<AuthenticationResult>>;