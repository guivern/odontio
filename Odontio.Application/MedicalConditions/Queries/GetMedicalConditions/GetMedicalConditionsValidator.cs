namespace Odontio.Application.MedicalConditions.Queries.GetMedicalConditions;

public class GetMedicalConditionsValidator: AbstractValidator<GetMedicalConditionsQuery>
{
    public GetMedicalConditionsValidator()
    {
        RuleFor(x => x.PatientId)
            .NotEmpty()
            .WithMessage("Patient id is required.");
        
        RuleFor(x => x.WorkspaceId)
            .NotEmpty()
            .WithMessage("Workspace id is required.");
    }
}