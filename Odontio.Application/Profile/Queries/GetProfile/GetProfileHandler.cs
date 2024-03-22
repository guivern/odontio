using System.Net;
using Odontio.Application.Common.Interfaces;

namespace Odontio.Application.Profile.Queries.GetProfile;

public class GetProfileHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService) : IRequestHandler<GetProfileQuery, ErrorOr<GetProfileResult>>
{
    public async Task<ErrorOr<GetProfileResult>> Handle(GetProfileQuery request, CancellationToken cancellationToken)
    {
        var userId = authService.GetCurrentUserId();
        if (request.Id != userId)
        {
            return Error.Custom((int)HttpStatusCode.Forbidden, "FORBIDDEN", "User is not authorized to perform this action.");
        }
        
        var entity = await context.Users.AsNoTracking()
            .Include(x => x.Workspace)
            .Include(x => x.Role)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        
        if (entity == null)
        {
            return Error.NotFound();
        }
        
        var result = mapper.Map<GetProfileResult>(entity);
        
        return result;
    }
}