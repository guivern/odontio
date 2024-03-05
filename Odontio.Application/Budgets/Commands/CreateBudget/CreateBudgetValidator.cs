namespace Odontio.Application.Budgets.Commands.CreateBudget;

public class CreateBudgetValidator : AbstractValidator<CreateBudgetCommand>
{
    public CreateBudgetValidator()
    {
        RuleFor(x => x.WorkspaceId)
            .NotEmpty()
            .WithMessage("Workspace is required");

        RuleFor(x => x.PatientId)
            .NotEmpty()
            .WithMessage("Patient is required");

        RuleFor(x => x.PatientTreatments)
            .NotEmpty()
            .WithMessage("PatientTreatments are required")
            .Must(x => x.All(y => y.Cost > -1))
            .WithMessage("Must be a positive value");
    }
}