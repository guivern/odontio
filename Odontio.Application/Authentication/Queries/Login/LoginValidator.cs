using FluentValidation;

namespace Odontio.Application.Authentication.Queries.Login;

public class LoginValidator: AbstractValidator<LoginQuery>
{
    public LoginValidator()
    {
        RuleFor(x => x.Username).NotEmpty();
        RuleFor(x => x.Password).NotEmpty();
    }
}