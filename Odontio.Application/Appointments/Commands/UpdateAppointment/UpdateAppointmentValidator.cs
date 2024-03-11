using Odontio.Application.Common.Interfaces;
using Odontio.Domain.Enums;

namespace Odontio.Application.Appointments.Commands.UpdateAppointment;

public class UpdateAppointmentValidator: AbstractValidator<UpdateAppointmentCommand>
{
    private readonly IApplicationDbContext _context;
    
    public UpdateAppointmentValidator(IApplicationDbContext context)
    {
        _context = context;
        RuleFor(x => x.MedicalRecords)
            .NotEmpty()
            .WithMessage("Medical records are required");

        // validate if PatientTreatment exists and belongs to the patient
        RuleForEach(x => x.MedicalRecords)
            .MustAsync(PatientTreatmentBelongsToPatient)
            .WithMessage("Patient treatment does not exist or does not belong to the patient");
        
        // validate if the PatientTreatment status is not finished
        RuleForEach(x => x.MedicalRecords)
            .MustAsync(PatientTreatmentNotFinished)
            .WithMessage("Patient treatment is finished");
        
        // validate if the MedicalRecord is new or belongs to the appointment
        RuleForEach(x => x.MedicalRecords)
            .MustAsync(MedicalRecordBelongsToAppointment)
            .WithMessage($"Medical record does not belong to the appointment");
        
    }

    private async Task<bool> MedicalRecordBelongsToAppointment(UpdateAppointmentCommand arg1, UpdateMedicalRecordCommand arg2, CancellationToken arg3)
    {
        if (arg2.Id == 0) return true; // new medical record
        
        var medicalRecord = await _context.MedicalRecords
            .Include(x => x.Appointment)
            .Where(x => x.Id == arg2.Id)
            .Where(x => x.AppointmentId == arg1.Id)
            .AsNoTracking()
            .FirstOrDefaultAsync(arg3);
        
        return medicalRecord is not null;
    }

    private async Task<bool> PatientTreatmentNotFinished(UpdateMedicalRecordCommand arg1, CancellationToken arg2)
    {
        var patientTreatment = await _context.PatientTreatments
            .Where(x => x.Id == arg1.PatientTreatmentId)
            .AsNoTracking()
            .FirstOrDefaultAsync(arg2);
        
        if (patientTreatment == null) return true; // already validated in PatientTreatmentBelongsToPatient

        return patientTreatment.Status != TreatmentStatus.Finished;
    }

    private async Task<bool> PatientTreatmentBelongsToPatient(UpdateAppointmentCommand arg1, UpdateMedicalRecordCommand arg2, CancellationToken arg3)
    {
        var patientTreatment = await _context.PatientTreatments
            .Include(x => x.Budget)
            .Where(x => x.Id == arg2.PatientTreatmentId)
            .Where(x => x.Budget.PatientId == arg1.PatientId)
            .AsNoTracking()
            .FirstOrDefaultAsync(arg3);
        
        return patientTreatment is not null;
    }
}