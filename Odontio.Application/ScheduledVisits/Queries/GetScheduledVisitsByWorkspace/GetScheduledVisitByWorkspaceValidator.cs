namespace Odontio.Application.ScheduledVisits.Queries.GetScheduledVisitsByWorkspace;

public class GetScheduledVisitByWorkspaceValidator : AbstractValidator<GetScheduledVisitsByWorkspaceQuery>
{
    public GetScheduledVisitByWorkspaceValidator()
    {
        RuleFor(x => x.WorkspaceId).NotEmpty();
    }
}