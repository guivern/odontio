namespace Odontio.Application.Budgets.Commands.UpdateBudget;

public class UpdateBudgetValidator : AbstractValidator<UpdateBudgetCommand>
{
    public UpdateBudgetValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Date).NotEmpty();
        RuleFor(x => x.WorkspaceId).NotEmpty();
        RuleFor(x => x.PatientId).NotEmpty();
        RuleFor(x => x.PatientTreatments)
            .NotEmpty()
            .WithMessage("PatientTreatments are required")
            .Must(x => x.All(y => y.Cost > -1))
            .WithMessage("Must be a positive value");
        
    }
}