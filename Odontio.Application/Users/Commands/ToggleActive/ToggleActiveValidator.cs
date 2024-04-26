namespace Odontio.Application.Users.Commands.ToggleActive;

public class ToggleActiveValidator: AbstractValidator<ToggleActiveCommand>
{
    public ToggleActiveValidator()
    {
        RuleFor(x => x.Id).NotNull().NotEmpty();
    }
}