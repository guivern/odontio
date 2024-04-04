using System.Net;
using Microsoft.AspNetCore.Http;
using Odontio.Application.Common.Attributes;
using Odontio.Application.Common.Interfaces;

namespace Odontio.Application.Common.Behaviors;

public class WorkspaceValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IWorkspaceResource
{
    private readonly IApplicationDbContext _context;
    private readonly IAuthService _authService;

    public WorkspaceValidationBehavior(IApplicationDbContext context, IAuthService authService)
    {
        _context = context;
        _authService = authService;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var requestType = request.GetType();
        var attributeExists = Attribute.IsDefined(requestType, typeof(ValidateWorkspaceAttribute));

        if (attributeExists)
        {
            /* 1- check if workspace exists */ 
            var workspaceExists = await _context.Workspaces.AsNoTracking()
                .AnyAsync(x => x.Id == request.WorkspaceId, cancellationToken);

            if (!workspaceExists)
            {
                return (dynamic)Error.NotFound(description: "Workspace not found");
            }

            /* 2- check if user has access to workspace */
            var userId = _authService.GetCurrentUserId();
            var user = await _context.Users.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == userId && x.IsActive, cancellationToken);

            // if user is an admin, allow access
            if (user != null && user.RoleId == (int)RolesEnum.Administrator)
            {
                return await next();
            }

            if (user == null || request.WorkspaceId != user?.WorkspaceId)
            {
                return (dynamic)Error.Custom((int)ErrorType.Unauthorized, "FORBIDDEN",
                    "User does not have access to this workspace");
            }
        }

        return await next();
    }
}