namespace Odontio.Application.Budgets.Commands.DeleteBudget;

public class DeleteBudgetValidator : AbstractValidator<DeleteBudgetCommand>
{
    public DeleteBudgetValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.WorkspaceId).NotEmpty();
        RuleFor(x => x.PatientId).NotEmpty();
    }
}