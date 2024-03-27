using Odontio.Application.Common.Interfaces;

namespace Odontio.Application.MedicalRecords.Commands.DeleteMedicalRecord;

public class DeleteMedicalRecordHandler(IApplicationDbContext context)
    : IRequestHandler<DeleteMedicalRecordCommand, ErrorOr<Unit>>
{
    public async Task<ErrorOr<Unit>> Handle(DeleteMedicalRecordCommand request, CancellationToken cancellationToken)
    {
        var medicalRecord = await context.MedicalRecords
            .Include(x => x.PatientTreatment)
            .ThenInclude(x => x.Budget)
            .Where(x => x.Id == request.Id)
            .Where(x => x.AppointmentId == request.AppointmentId)
            .Where(x => x.PatientTreatment.Budget.PatientId == request.PatientId)
            .FirstOrDefaultAsync(cancellationToken);

        if (medicalRecord is null)
        {
            return Error.NotFound(description: "Medical record not found");
        }

        // count and validate how many medical records are in the appointment
        var appointment = await context.Appointments
            .Where(x => x.Id == request.AppointmentId && x.PatientId == request.PatientId)
            .Include(x => x.MedicalRecords)
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);

        if (appointment.MedicalRecords.Count == 1)
        {
            return Error.Validation(description: "An appointment must have at least one medical record");
        }

        context.MedicalRecords.Remove(medicalRecord);

        await context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}