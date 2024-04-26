using Odontio.Application.Common.Attributes;
using Odontio.Application.Common.Interfaces;
using Odontio.Application.Users.Common;

namespace Odontio.Application.Users.Commands.CreateUser;

[RolesAuthorize(nameof(RolesEnum.Administrator))]
public class CreateUserCommand : IRequest<ErrorOr<UpsertUserResult>>
{
    public string Username { get; set; } = null!;
    public string? Password { get; set; }
    public string? ConfirmPassword { get; set; }
    public string? Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? PhotoUrl { get; set; }
    public long? WorkspaceId { get; set; }
    public long RoleId { get; set; }
    public bool IsDoctor { get; set; }
}