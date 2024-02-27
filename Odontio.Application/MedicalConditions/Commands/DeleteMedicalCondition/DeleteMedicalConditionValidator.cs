namespace Odontio.Application.MedicalConditions.Common.DeleteMedicalCondition;

public class DeleteMedicalConditionValidator: AbstractValidator<DeleteMedicalConditionCommand>
{
    public DeleteMedicalConditionValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Medical condition id is required.");
        
        RuleFor(x => x.WorkspaceId)
            .NotEmpty()
            .WithMessage("Workspace id is required.");
        
        RuleFor(x => x.PatientId)
            .NotEmpty()
            .WithMessage("Patient id is required.");
    }
}