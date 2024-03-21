using Odontio.Application.Authentication.Common;

namespace Odontio.Application.Authentication.Commands.ChangePassword;

public class ChangePasswordCommand : IRequest<ErrorOr<AuthenticationResult>>
{
    public string Username { get; set; }
    public string OldPassword { get; set; }
    public string NewPassword { get; set; }
    public string ConfirmPassword { get; set; }
}