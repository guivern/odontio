using Odontio.Application.Common.Interfaces;
using Odontio.Application.ScheduledVisits.Common;

namespace Odontio.Application.ScheduledVisits.Queries.GetScheduledVisitsByPatient;

public class GetScheduledVisitsHandler(IApplicationDbContext context): IRequestHandler<GetScheduledVisitsQuery, IEnumerable<ScheduledVisitDto>>
{
    public async Task<IEnumerable<ScheduledVisitDto>> Handle(GetScheduledVisitsQuery query, CancellationToken cancellationToken)
    {
        var visits = await context.ScheduledVisits
            .Where(x => x.PatientId == query.PatientId)
            .ProjectToType<ScheduledVisitDto>()
            .ToListAsync(cancellationToken);

        return visits;
    }
}