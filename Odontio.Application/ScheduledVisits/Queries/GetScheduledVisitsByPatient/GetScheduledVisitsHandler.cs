using Odontio.Application.Common.Interfaces;
using Odontio.Application.ScheduledVisits.Common;

namespace Odontio.Application.ScheduledVisits.Queries.GetScheduledVisitsByPatient;

public class GetScheduledVisitsHandler(IApplicationDbContext context): IRequestHandler<GetScheduledVisitsQuery, IEnumerable<UpsertScheduledVisitResult>>
{
    public async Task<IEnumerable<UpsertScheduledVisitResult>> Handle(GetScheduledVisitsQuery query, CancellationToken cancellationToken)
    {
        var visits = await context.ScheduledVisits
            .Where(x => x.PatientId == query.PatientId)
            .ProjectToType<UpsertScheduledVisitResult>()
            .ToListAsync(cancellationToken);

        return visits;
    }
}