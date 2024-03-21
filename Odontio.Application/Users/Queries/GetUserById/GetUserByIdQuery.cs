using Odontio.Application.Common.Attributes;

namespace Odontio.Application.Users.Queries.GetUserById;

[RolesAuthorize(nameof(RolesEnum.Administrator))]
public class GetUserByIdQuery : IRequest<ErrorOr<GetUserByIdResult>>
{
    public long Id { get; set; }
}