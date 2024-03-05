namespace Odontio.Application.Budgets.Queries.GetBudgetsByWorkspace;

public class GetBudgetsByWorkspaceValidator : AbstractValidator<GetBudgetsByWorkspaceQuery>
{
    public GetBudgetsByWorkspaceValidator()
    {
        RuleFor(x => x.WorkspaceId).NotEmpty();
    }
}