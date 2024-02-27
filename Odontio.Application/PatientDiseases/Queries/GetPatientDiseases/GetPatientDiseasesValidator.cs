namespace Odontio.Application.PatientDiseases.Queries.GetPatientDiseases;

public class GetPatientDiseasesValidator: AbstractValidator<GetPatientDiseasesQuery>
{
    public GetPatientDiseasesValidator()
    {
        RuleFor(x => x.PatientId).NotEmpty();
        RuleFor(x => x.WorkspaceId).NotEmpty();
    }
}