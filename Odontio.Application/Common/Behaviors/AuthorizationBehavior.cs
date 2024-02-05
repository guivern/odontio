using System.Net;
using System.Security.Authentication;
using System.Security.Claims;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Odontio.Application.Common.Exceptions;
using Odontio.Application.Common.Interfaces;

namespace Odontio.Application.Common.Behaviors;

public class AuthorizationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TResponse : IErrorOr
{
    private readonly IAuthorizationService _authorizationService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthorizationBehavior(IAuthorizationService authorizationService, IHttpContextAccessor httpContextAccessor)
    {
        _authorizationService = authorizationService;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (request is IAuthorizeable authorizeable)
        {
            var user = _httpContextAccessor.HttpContext.User;
            
            // validate all roles
            foreach (var role in authorizeable.Roles)
            {
                var authorizationResult = await _authorizationService.AuthorizeAsync(user, role);
                
                if (authorizationResult.Succeeded)
                {
                    return await next();
                }
            }
            
            // throw new UnauthorizedException();
            return (dynamic) Error.Custom(code: "FORBIDEN", description: "User is not authorized to perform this action.",
                type: (int)HttpStatusCode.Forbidden);
            
        }

        return await next();
    }
}
