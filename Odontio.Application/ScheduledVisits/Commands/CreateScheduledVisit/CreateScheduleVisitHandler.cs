using Odontio.Application.Common.Interfaces;
using Odontio.Application.ScheduledVisits.Common;
using Odontio.Domain.Entities;

namespace Odontio.Application.ScheduledVisits.Commands.CreateScheduledVisit;

public class CreateScheduleVisitHandler(IApplicationDbContext context, IMapper mapper)
    : IRequestHandler<CreateScheduleVisitCommand, ErrorOr<ScheduledVisitDto>>
{
    public async Task<ErrorOr<ScheduledVisitDto>> Handle(CreateScheduleVisitCommand command, CancellationToken cancellationToken)
    {
        var entity = mapper.Map<ScheduledVisit>(command);

        context.ScheduledVisits.Add(entity);

        await context.SaveChangesAsync(cancellationToken);

        var result = mapper.Map<ScheduledVisitDto>(entity);
        
        return result;
    }
}