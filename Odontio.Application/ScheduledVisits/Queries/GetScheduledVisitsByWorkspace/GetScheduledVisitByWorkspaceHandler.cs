using Odontio.Application.Common.Interfaces;

namespace Odontio.Application.ScheduledVisits.Queries.GetScheduledVisitsByWorkspace;

public class GetScheduledVisitByWorkspaceHandler(IApplicationDbContext context): IRequestHandler<GetScheduledVisitsByWorkspaceQuery, IEnumerable<GetScheduledVisitByWorkspaceResult>>
{
    public async Task<IEnumerable<GetScheduledVisitByWorkspaceResult>> Handle(GetScheduledVisitsByWorkspaceQuery query, CancellationToken cancellationToken)
    {
        var visits = await context.ScheduledVisits
            .Include(x => x.Patient)
            .Where(x => x.Patient.WorkspaceId == query.WorkspaceId)
            .ProjectToType<GetScheduledVisitByWorkspaceResult>()
            .ToListAsync(cancellationToken);

        return visits;
    }
}