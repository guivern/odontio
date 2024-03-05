namespace Odontio.Application.Patients.Queries.GetPatients;

public class GetPatientsValidator : AbstractValidator<GetPatientsQuery>
{
    public GetPatientsValidator()
    {
        RuleFor(x => x.WorkspaceId).NotEmpty();
    }
}