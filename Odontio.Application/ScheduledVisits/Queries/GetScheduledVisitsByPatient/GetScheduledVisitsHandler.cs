using Odontio.Application.Common.Interfaces;
using Odontio.Application.ScheduledVisits.Common;

namespace Odontio.Application.ScheduledVisits.Queries.GetScheduledVisitsByPatient;

public class GetScheduledVisitsHandler(IApplicationDbContext context): IRequestHandler<GetScheduledVisitsQuery, IEnumerable<UpsertScheduledVisitResult>>
{
    public async Task<IEnumerable<UpsertScheduledVisitResult>> Handle(GetScheduledVisitsQuery request, CancellationToken cancellationToken)
    {
        var query = context.ScheduledVisits
            .Include(x => x.Patient)
            .Where(x => x.PatientId == request.PatientId)
            .ProjectToType<UpsertScheduledVisitResult>()
            .AsNoTracking()
            .AsQueryable();

        if (request.DateRange.StartDate != null)
        {
            query = query.Where(x => DateOnly.FromDateTime(x.Date.Date) >= request.DateRange.StartDate);
        }
        
        if (request.DateRange.EndDate != null)
        {
            query = query.Where(x => DateOnly.FromDateTime(x.Date.Date) <= request.DateRange.EndDate);
        }
        
        var result = await query.ToListAsync(cancellationToken);

        return result;
    }
}