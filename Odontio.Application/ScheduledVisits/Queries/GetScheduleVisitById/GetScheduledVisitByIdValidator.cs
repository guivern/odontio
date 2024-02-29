namespace Odontio.Application.ScheduledVisits.Queries.GetScheduleVisitById;

public class GetScheduledVisitByIdValidator : AbstractValidator<GetScheduleVisitByIdQuery>
{
    public GetScheduledVisitByIdValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.PatientId).NotEmpty();
        RuleFor(x => x.WorkspaceId).NotEmpty();
    }
}