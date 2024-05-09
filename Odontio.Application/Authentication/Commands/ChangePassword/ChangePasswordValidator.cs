using Odontio.Application.Common.Interfaces;

namespace Odontio.Application.Authentication.Commands.ChangePassword;

public class ChangePasswordValidator : AbstractValidator<ChangePasswordCommand>
{
    private readonly IApplicationDbContext _context;

    public ChangePasswordValidator(IApplicationDbContext context)
    {
        _context = context;
        RuleFor(v => v.OldPassword)
            .NotEmpty().WithMessage("Es requerido.");
        RuleFor(v => v.NewPassword)
            .NotEqual(v => v.OldPassword).WithMessage("Debe ser diferente al anterior.")
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$")
            .WithMessage("Debe contener al menos 8 caracteres, una mayúscula, una minúscula y un número.")
            .NotEmpty();
        RuleFor(x => x.ConfirmPassword)
            .NotEmpty()
            .Equal(x => x.NewPassword).WithMessage("No coincide.");
    }
}