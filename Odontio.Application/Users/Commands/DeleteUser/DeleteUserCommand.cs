using Odontio.Application.Common.Attributes;

namespace Odontio.Application.Users.Commands.DeleteUser;

[RolesAuthorize(nameof(RolesEnum.Administrator))]
public class DeleteUserCommand : IRequest<ErrorOr<Unit>>
{
    public long Id { get; set; }
}