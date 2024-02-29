namespace Odontio.Application.ScheduledVisits.Commands.DeleteScheduledVisit;

public class DeleteScheduledVisitValidator: AbstractValidator<DeleteScheduledVisitCommand>
{
    public DeleteScheduledVisitValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.WorkspaceId).NotEmpty();
        RuleFor(x => x.PatientId).NotEmpty();
    }
}