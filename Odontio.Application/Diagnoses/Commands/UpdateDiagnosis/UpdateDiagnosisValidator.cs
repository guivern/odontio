namespace Odontio.Application.Diagnoses.Commands.UpdateDiagnosis;

public class UpdateDiagnosisValidator: AbstractValidator<UpdateDiagnosisCommand>
{
    public UpdateDiagnosisValidator()
    {
        RuleFor(x => x.Date).NotEmpty();
        RuleFor(x => x.Description).NotEmpty();
    }
}