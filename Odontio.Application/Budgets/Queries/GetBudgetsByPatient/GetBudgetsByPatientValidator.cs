namespace Odontio.Application.Budgets.Queries.GetBudgetsByPatient;

public class GetBudgetsByPatientValidator : AbstractValidator<GetBudgetsByPatientQuery>
{
    public GetBudgetsByPatientValidator()
    {
        RuleFor(x => x.PatientId).NotEmpty();
        RuleFor(x => x.WorkspaceId).NotEmpty();
    }
}