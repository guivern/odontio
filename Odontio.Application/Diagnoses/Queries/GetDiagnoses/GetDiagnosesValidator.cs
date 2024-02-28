namespace Odontio.Application.Diagnoses.Queries.GetDiagnoses;

public class GetDiagnosesValidator: AbstractValidator<GetDiagnosesQuery>
{
    public GetDiagnosesValidator()
    {
        RuleFor(x => x.PatientId).NotEmpty();
        RuleFor(x => x.WorkspaceId).NotEmpty();
    }
}