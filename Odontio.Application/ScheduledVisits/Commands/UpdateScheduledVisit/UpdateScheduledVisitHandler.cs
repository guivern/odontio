using Odontio.Application.Common.Interfaces;
using Odontio.Application.ScheduledVisits.Common;

namespace Odontio.Application.ScheduledVisits.Commands.UpdateScheduledVisit;

public class UpdateScheduledVisitHandler(IApplicationDbContext context, IMapper mapper): IRequestHandler<UpdateScheduledVisitCommand, ErrorOr<UpsertScheduledVisitResult>>
{
    public async Task<ErrorOr<UpsertScheduledVisitResult>> Handle(UpdateScheduledVisitCommand command, CancellationToken cancellationToken)
    {
        var visit = await context.ScheduledVisits
            .Where(x => x.Id == command.Id)
            .Where(x => x.PatientId == command.PatientId)
            .FirstOrDefaultAsync(cancellationToken);

        if (visit == null)
        {
            return Error.NotFound(description: "Visit not found");
        }

        visit.Date = command.Date;
        visit.Description = command.Description;
        
        context.ScheduledVisits.Entry(visit).State = EntityState.Modified;

        try
        {
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException)
        {
            return Error.Conflict(description: "The visit was modified by another user");
        }
        
        var result = mapper.Map<UpsertScheduledVisitResult>(visit);

        return result;
    }
}