namespace Odontio.Application.Users.Commands.ResetPassword;

public class ResetPasswordValidator : AbstractValidator<ResetPasswordCommand>
{
    public ResetPasswordValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Password)
            .NotEmpty()
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$")
            .WithMessage("El password debe tener al menos 8 caracteres, una mayúscula, una minúscula y un número.");
        RuleFor(x => x.ConfirmPassword).Equal(x => x.Password)
            .WithMessage("No coincide.");
    }
}