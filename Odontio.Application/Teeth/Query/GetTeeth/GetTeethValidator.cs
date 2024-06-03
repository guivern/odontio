using Odontio.Domain.Enums;

namespace Odontio.Application.Teeth.Query.GetTeeth;

public class GetTeethValidator : AbstractValidator<GetTeethQuery>
{
    public GetTeethValidator()
    {
        RuleFor(x => x.Odontogram)
            .Must(ValidOdontogram).WithMessage("Odontogram no es valido. Debe ser Niño o Adulto.");
    }

    private bool ValidOdontogram(string? arg)
    {
        return arg is null or nameof(OdontogramType.Niño) or nameof(OdontogramType.Adulto);
    }
}