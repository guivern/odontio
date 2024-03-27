using Odontio.Application.Common.Interfaces;

namespace Odontio.Application.MedicalNotes.Commands.DeleteMedicalNote;

public class DeleteMedicalNoteHandler(IApplicationDbContext context)
    : IRequestHandler<DeleteMedicalNoteCommand, ErrorOr<Unit>>
{
    public async Task<ErrorOr<Unit>> Handle(DeleteMedicalNoteCommand request, CancellationToken cancellationToken)
    {
        var medicalNote = await context.MedicalNotes
            .Include(x => x.PatientTreatment)
            .ThenInclude(x => x.Budget)
            .Where(x => x.Id == request.Id)
            .Where(x => x.AppointmentId == request.AppointmentId)
            .Where(x => x.PatientTreatment.Budget.PatientId == request.PatientId)
            .FirstOrDefaultAsync(cancellationToken);

        if (medicalNote is null)
        {
            return Error.NotFound(description: "Medical record not found");
        }

        // count and validate how many medical records are in the appointment
        var appointment = await context.Appointments
            .Where(x => x.Id == request.AppointmentId && x.PatientId == request.PatientId)
            .Include(x => x.MedicalNotes)
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);

        if (appointment.MedicalNotes.Count == 1)
        {
            return Error.Validation(description: "An appointment must have at least one medical record");
        }

        context.MedicalNotes.Remove(medicalNote);

        await context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}