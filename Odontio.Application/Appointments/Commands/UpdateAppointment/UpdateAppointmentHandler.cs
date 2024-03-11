using Odontio.Application.Appointments.Common;
using Odontio.Application.Common.Interfaces;

namespace Odontio.Application.Appointments.Commands.UpdateAppointment;

public class UpdateAppointmentHandler(IApplicationDbContext context, IMapper mapper, IDateTimeProvider dateTimeProvider) : IRequestHandler<UpdateAppointmentCommand, ErrorOr<UpsertAppointmentResult>>
{
    public async Task<ErrorOr<UpsertAppointmentResult>> Handle(UpdateAppointmentCommand request, CancellationToken cancellationToken)
    {
        var appointment = await context.Appointments
            .Include(x => x.MedicalRecords)
            .ThenInclude(x => x.PatientTreatment)
            .ThenInclude(x => x.Treatment)
            .Include(x => x.Patient)
            .Where(x => x.PatientId == request.PatientId)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (appointment == null)
        {
            return Error.NotFound(description: "Appointment not found");
        }

        mapper.Map(request, appointment);
        appointment.Date = request.Date ?? dateTimeProvider.UtcNow;
        
        context.Appointments.Entry(appointment).State = EntityState.Modified;

        try
        {
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException)
        {
            return Error.Conflict(description: "The Appointment was modified by another user");
        }

        var result = mapper.Map<UpsertAppointmentResult>(appointment);
        return result;
    }
}