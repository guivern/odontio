namespace Odontio.Application.Diagnoses.Commands.DeleteDiagnosis;

public class DeleteDiagnosisValidator: AbstractValidator<DeleteDiagnosisCommand>
{
    public DeleteDiagnosisValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.WorkspaceId).NotEmpty();
        RuleFor(x => x.PatientId).NotEmpty();
    }
}