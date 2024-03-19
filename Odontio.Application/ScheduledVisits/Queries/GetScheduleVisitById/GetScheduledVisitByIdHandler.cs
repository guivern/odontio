using Odontio.Application.Common.Interfaces;
using Odontio.Application.ScheduledVisits.Common;

namespace Odontio.Application.ScheduledVisits.Queries.GetScheduleVisitById;

public class GetScheduledVisitByIdHandler(IApplicationDbContext context, IMapper mapper)
    : IRequestHandler<GetScheduleVisitByIdQuery, ErrorOr<UpsertScheduledVisitResult>>
{
    public async Task<ErrorOr<UpsertScheduledVisitResult>> Handle(GetScheduleVisitByIdQuery query,
        CancellationToken cancellationToken)
    {
        var entity = await context.ScheduledVisits
            .AsNoTracking()
            .Where(x => x.Id == query.Id && x.PatientId == query.PatientId)
            .ProjectToType<UpsertScheduledVisitResult>()
            .FirstOrDefaultAsync(cancellationToken);

        if (entity == null)
        {
            return Error.NotFound(description: "Scheduled visit not found");
        }

        return entity;
    }
}