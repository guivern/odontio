namespace Odontio.Application.ScheduledVisits.Queries.GetScheduledVisitsByPatient;

public class GetScheduledVisitsValidator: AbstractValidator<GetScheduledVisitsQuery>
{
    public GetScheduledVisitsValidator()
    {
        RuleFor(x => x.WorkspaceId).NotEmpty();
        RuleFor(x => x.PatientId).NotEmpty();
    }
}