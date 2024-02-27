using System.Net;
using Microsoft.AspNetCore.Http;
using Odontio.Application.Common.Attributes;
using Odontio.Application.Common.Interfaces;

namespace Odontio.Application.Common.Behaviors;

public class WorkspaceValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest: IWorkspaceResource
{
    private readonly IApplicationDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public WorkspaceValidationBehavior(IApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var requestType = request.GetType();
        var attributeExists = Attribute.IsDefined(requestType, typeof(ValidateWorkspaceAttribute));

        if (attributeExists)
        {
            // check if workspace exists
            var workspaceExists = await _context.Workspaces.AsNoTracking()
                .AnyAsync(x => x.Id == request.WorkspaceId, cancellationToken);
            
            if (!workspaceExists)
            {
                return (dynamic)Error.NotFound(description: "Workspace not found");
            }
            
            // check if user has access to workspace
            var user = _httpContextAccessor.HttpContext?.User;
            var userWorkspaceId = user?.FindFirst("WorkspaceId")?.Value;
            
            if (userWorkspaceId == null || request.WorkspaceId != long.Parse(userWorkspaceId))
            {
                return (dynamic)Error.Custom((int)HttpStatusCode.Forbidden, "FORBIDDEN",
                    "User does not have access to this workspace");
            }
        }

        return await next();
    }
}
