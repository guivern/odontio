using Odontio.Application.Common.Interfaces;
using Odontio.Application.ScheduledVisits.Common;

namespace Odontio.Application.ScheduledVisits.Queries.GetScheduledVisitsByWorkspace;

public class GetScheduledVisitByWorkspaceHandler(IApplicationDbContext context)
    : IRequestHandler<GetScheduledVisitsByWorkspaceQuery, ErrorOr<List<GetScheduledVisitResult>>>
{
    public async Task<ErrorOr<List<GetScheduledVisitResult>>> Handle(
        GetScheduledVisitsByWorkspaceQuery request, CancellationToken cancellationToken)
    {
        var query = context.ScheduledVisits
            .Include(x => x.Patient)
            .Where(x => x.Patient.WorkspaceId == request.WorkspaceId)
            .ProjectToType<GetScheduledVisitResult>()
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