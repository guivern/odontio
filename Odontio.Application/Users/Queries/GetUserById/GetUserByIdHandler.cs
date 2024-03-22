using Odontio.Application.Common.Interfaces;

namespace Odontio.Application.Users.Queries.GetUserById;

public class GetUserByIdHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<GetUserByIdQuery, ErrorOr<GetUserByIdResult>>
{
    public async Task<ErrorOr<GetUserByIdResult>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await context.Users
            .AsNoTracking()
            .Include(u => u.Workspace)
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);

        if (user == null)
        {
            return Error.NotFound(description: "User not found");
        }

        var result = mapper.Map<GetUserByIdResult>(user);

        return result;
    }
}