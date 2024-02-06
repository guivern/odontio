using System.Net;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Odontio.Application.Common.Attributes;

namespace Odontio.Application.Common.Behaviors;

public class AuthorizationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TResponse : IErrorOr
{
    private readonly IAuthorizationService _authorizationService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthorizationBehavior(IAuthorizationService authorizationService, IHttpContextAccessor httpContextAccessor)
    {
        _authorizationService = authorizationService;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var authorizeAttributes = request.GetType().GetCustomAttributes<RolesAuthorizeAttribute>();
        if (authorizeAttributes.Any())
        {
            var user = _httpContextAccessor.HttpContext.User;

            // validate all roles
            foreach (var attribute in authorizeAttributes)
            {
                foreach (var role in attribute.Roles)
                {
                    var authorizationResult = await _authorizationService.AuthorizeAsync(user, role);
                    if (authorizationResult.Succeeded)
                    {
                        return await next();
                    }
                }
            }

            // throw new UnauthorizedException();
            return (dynamic)Error.Custom((int)HttpStatusCode.Forbidden, "FORBIDDEN",
                "User is not authorized to perform this action.");
        }

        return await next();
    }
}