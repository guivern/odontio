using Odontio.Application.Authentication.Common;

namespace Odontio.Application.Authentication.Commands.ChangePassword;

public class ChangePasswordCommand : IRequest<ErrorOr<AuthenticationResult>>
{
    public long Id { get; set; }
    public string OldPassword { get; set; }
    public string NewPassword { get; set; }
    public string ConfirmPassword { get; set; }
}