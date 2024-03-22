using Odontio.Application.Common.Interfaces;

namespace Odontio.Application.Authentication.Commands.ChangePassword;

public class ChangePasswordValidator : AbstractValidator<ChangePasswordCommand>
{
    private readonly IApplicationDbContext _context;

    public ChangePasswordValidator(IApplicationDbContext context)
    {
        _context = context;
        RuleFor(v => v.OldPassword)
            .NotEmpty().WithMessage("Old password is required.");
        RuleFor(v => v.NewPassword)
            .NotEqual(v => v.OldPassword).WithMessage("New password must be different from the old password.")
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$")
            .WithMessage("Password must contain at least one uppercase letter, one lowercase letter, and one number.")
            .NotEmpty();
        RuleFor(x => x.ConfirmPassword)
            .NotEmpty()
            .Equal(x => x.NewPassword).WithMessage("Passwords do not match.");
    }
}