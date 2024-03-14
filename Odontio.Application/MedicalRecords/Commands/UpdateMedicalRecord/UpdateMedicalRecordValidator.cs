using Odontio.Application.Common.Interfaces;

namespace Odontio.Application.MedicalRecords.Commands.UpdateMedicalRecord;

public class UpdateMedicalRecordValidator : AbstractValidator<UpdateMedicalRecordCommand>
{
    private readonly IApplicationDbContext _context;
    
    public UpdateMedicalRecordValidator(IApplicationDbContext context)
    {
        _context = context;
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.WorkspaceId).NotEmpty();
        RuleFor(x => x.PatientId).NotEmpty();
        RuleFor(x => x.AppointmentId).NotEmpty();
        RuleFor(x => x.PatientTreatmentId).NotEmpty()
            .MustAsync(PatientTreatmentBelongsToPatient).WithMessage("Patient treatment not found");
    }

    private async Task<bool> PatientTreatmentBelongsToPatient(UpdateMedicalRecordCommand arg1, long arg2, CancellationToken arg3)
    {
        var patientTreatment = await _context.PatientTreatments
            .Include(x => x.Budget)
            .Where(x => x.Id == arg2)
            .Where(x => x.Budget.PatientId == arg1.PatientId)
            .AsNoTracking()
            .FirstOrDefaultAsync(arg3);
        
        if (patientTreatment == null) return false; // patient treatment not found
        
        // validate if the appointment exists and the patient treatment is related to it
        var appointment = await _context.Appointments
            .Where(x => x.Id == arg1.AppointmentId)
            .Where(x => x.MedicalRecords.Any(x => x.PatientTreatmentId == arg2))
            .AsNoTracking()
            .FirstOrDefaultAsync(arg3);
        
        return appointment != null;
    }
}