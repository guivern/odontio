using Odontio.Application.Common.Interfaces;

namespace Odontio.Application.ScheduledVisits.Commands.DeleteScheduledVisit;

public class DeleteScheduledVisitHandler(IApplicationDbContext context): IRequestHandler<DeleteScheduledVisitCommand, ErrorOr<Unit>>
{
    public async Task<ErrorOr<Unit>> Handle(DeleteScheduledVisitCommand command, CancellationToken cancellationToken)
    {
        var visit = await context.ScheduledVisits
            .Where(x => x.Id == command.Id)
            .Where(x => x.PatientId == command.PatientId)
            .FirstOrDefaultAsync(cancellationToken);

        if (visit == null)
        {
            return Error.NotFound(description: "Visit not found");
        }

        context.ScheduledVisits.Remove(visit);

        await context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}