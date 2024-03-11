using Odontio.Application.Appointments.Common;
using Odontio.Application.Common.Interfaces;
using Odontio.Domain.Entities;

namespace Odontio.Application.Appointments.Commands.CreateAppointment;

public class CreateAppointmentHandler(IApplicationDbContext context, IMapper mapper, IDateTimeProvider dateTimeProvider)
    : IRequestHandler<CreateAppointmentCommand, ErrorOr<UpsertAppointmentResult>>
{
    public async Task<ErrorOr<UpsertAppointmentResult>> Handle(CreateAppointmentCommand request,
        CancellationToken cancellationToken)
    {
        var appointment = mapper.Map<Appointment>(request);
        appointment.Date = request.Date ?? dateTimeProvider.UtcNow;

        context.Appointments.Add(appointment);
        await context.SaveChangesAsync(cancellationToken);

        var result = mapper.Map<UpsertAppointmentResult>(appointment);
        return result;
    }
}