using Odontio.Application.Common.Interfaces;
using Odontio.Application.ScheduledVisits.Common;
using Odontio.Domain.Entities;

namespace Odontio.Application.ScheduledVisits.Commands.CreateScheduledVisit;

public class CreateScheduleVisitHandler(IApplicationDbContext context, IMapper mapper)
    : IRequestHandler<CreateScheduleVisitCommand, ErrorOr<UpsertScheduledVisitResult>>
{
    public async Task<ErrorOr<UpsertScheduledVisitResult>> Handle(CreateScheduleVisitCommand command, CancellationToken cancellationToken)
    {
        var entity = mapper.Map<ScheduledVisit>(command);

        context.ScheduledVisits.Add(entity);

        await context.SaveChangesAsync(cancellationToken);

        var result = mapper.Map<UpsertScheduledVisitResult>(entity);
        
        return result;
    }
}