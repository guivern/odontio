using Odontio.Application.Appointments.Common;
using Odontio.Application.Common.Interfaces;
using Odontio.Domain.Entities;
using Odontio.Domain.Enums;

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
        
        foreach (var medicalRecord in appointment.MedicalRecords)
        {
            var patientTreatment = await context.PatientTreatments
                .Include(x => x.Budget)
                .FirstAsync(x => x.Id == medicalRecord.PatientTreatmentId, cancellationToken);
            
            patientTreatment.Status = TreatmentStatus.InProgress;
            patientTreatment.Budget.Status = BudgetStatus.Approved;
        }
        
        await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            await context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }
        catch (Exception)
        {
            await transaction.RollbackAsync(cancellationToken);
            return Error.Unexpected("An error occurred while creating the appointment. Please try again.");
        }

        var result = mapper.Map<UpsertAppointmentResult>(appointment);
        return result;
    }
}