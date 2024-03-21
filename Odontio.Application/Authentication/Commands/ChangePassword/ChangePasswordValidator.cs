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
            .MinimumLength(8)
            .NotEmpty();
        RuleFor(x => x.ConfirmPassword)
            .NotEmpty()
            .Equal(x => x.NewPassword).WithMessage("Passwords do not match.");
    }
}