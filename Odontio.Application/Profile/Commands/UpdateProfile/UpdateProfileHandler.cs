using System.Net;
using Odontio.Application.Common.Interfaces;

namespace Odontio.Application.Profile.Commands.UpdateProfile;

public class UpdateProfileHandler(
    IApplicationDbContext context,
    IMapper mapper,
    IAuthService authService) : IRequestHandler<UpdateProfileCommand, ErrorOr<UpdateProfileResult>>
{
    public async Task<ErrorOr<UpdateProfileResult>> Handle(UpdateProfileCommand request,
        CancellationToken cancellationToken)
    {
        // validate if the userId from the request is the same as the userId from the token
        var userId = authService.GetCurrentUserId();
        if (request.Id != userId)
        {
            return Error.Custom((int)HttpStatusCode.Forbidden, "FORBIDDEN",
                "User is not authorized to perform this action.");
        }

        var entity = await context.Users.FindAsync(request.Id);

        if (entity == null)
        {
            return Error.NotFound();
        }

        entity = mapper.Map(request, entity);

        context.Users.Entry(entity).State = EntityState.Modified;

        try
        {
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException)
        {
            return Error.Conflict(description: "The User was modified by another user");
        }

        var result = mapper.Map<UpdateProfileResult>(entity);

        return result;
    }
}