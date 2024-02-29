using Odontio.Application.Common.Interfaces;
using Odontio.Application.ScheduledVisits.Common;

namespace Odontio.Application.ScheduledVisits.Queries.GetScheduleVisitById;

public class GetScheduledVisitByIdHandler(IApplicationDbContext context, IMapper mapper)
    : IRequestHandler<GetScheduleVisitByIdQuery, ErrorOr<ScheduledVisitDto>>
{
    public async Task<ErrorOr<ScheduledVisitDto>> Handle(GetScheduleVisitByIdQuery query,
        CancellationToken cancellationToken)
    {
        var entity = await context.ScheduledVisits
            .Where(x => x.Id == query.Id && x.PatientId == query.PatientId)
            .ProjectToType<ScheduledVisitDto>()
            .FirstOrDefaultAsync(cancellationToken);

        if (entity == null)
        {
            return Error.NotFound(description: "Scheduled visit not found");
        }

        return entity;
    }
}