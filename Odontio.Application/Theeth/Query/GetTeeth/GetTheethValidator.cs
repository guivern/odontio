using Odontio.Domain.Enums;

namespace Odontio.Application.Theeth.Query.GetTeeth;

public class GetTheethValidator : AbstractValidator<GetTheethQuery>
{
    public GetTheethValidator()
    {
        RuleFor(x => x.Odontogram)
            .Must(ValidOdontogram).WithMessage("Odontogram is not valid. Valid values are: 'Child', 'Adult' or null");
    }

    private bool ValidOdontogram(string arg)
    {
        return arg == null || arg == nameof(OdontogramType.Niño) || arg == nameof(OdontogramType.Adulto);
    }
}