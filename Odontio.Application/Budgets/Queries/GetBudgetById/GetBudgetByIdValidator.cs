namespace Odontio.Application.Budgets.Queries.GetBudgetById;

public class GetBudgetByIdValidator: AbstractValidator<GetBudgetByIdQuery>
{
    public GetBudgetByIdValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.PatientId).NotEmpty();
        RuleFor(x => x.WorkspaceId).NotEmpty();
    }
}