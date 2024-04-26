using Odontio.Application.Common.Attributes;

namespace Odontio.Application.Users.Commands.ResetPassword;

[RolesAuthorize(nameof(RolesEnum.Administrator))]
public class ResetPasswordCommand : IRequest<ErrorOr<Unit>>
{
    public long Id { get; set; }
    public string Password { get; set; } = null!;
    public string ConfirmPassword { get; set; } = null!;
}