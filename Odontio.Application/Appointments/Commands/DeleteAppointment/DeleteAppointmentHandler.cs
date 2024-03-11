using Odontio.Application.Common.Interfaces;

namespace Odontio.Application.Appointments.Commands.DeleteAppointment;

public class DeleteAppointmentHandler(IApplicationDbContext context) : IRequestHandler<DeleteAppointmentCommand, ErrorOr<Unit>>
{
    public async Task<ErrorOr<Unit>> Handle(DeleteAppointmentCommand request, CancellationToken cancellationToken)
    {
        var appointment = await context.Appointments
            .Where(x => x.PatientId == request.PatientId)
            .Where(x => x.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (appointment == null)
        {
            return Error.NotFound(description: "Appointment not found");
        }

        context.Appointments.Remove(appointment);

        await context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}