namespace Odontio.Application.Diagnoses.Queries.GetDiagnosisById;

public class GetDiagnosisByIdValidator: AbstractValidator<GetDiagnosisByIdQuery>
{
    public GetDiagnosisByIdValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.PatientId).NotEmpty();
        RuleFor(x => x.WorkspaceId).NotEmpty();
    }
}