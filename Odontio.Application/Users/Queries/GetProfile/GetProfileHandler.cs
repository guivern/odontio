using Odontio.Application.Common.Interfaces;

namespace Odontio.Application.Users.Queries.GetProfile;

public class GetProfileHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService) : IRequestHandler<GetProfileQuery, ErrorOr<GetProfileResult>>
{
    public async Task<ErrorOr<GetProfileResult>> Handle(GetProfileQuery request, CancellationToken cancellationToken)
    {
        var userId = authService.GetCurrentUserId();
        if (request.Id != userId)
        {
            return Error.Unauthorized();
        }
        
        var entity = await context.Users.FindAsync(request.Id);
        
        if (entity == null || !entity.IsActive)
        {
            return Error.NotFound();
        }
        
        var result = mapper.Map<GetProfileResult>(entity);
        
        return result;
    }
}