using Odontio.Application.Common.Interfaces;
using Odontio.Domain.Enums;

namespace Odontio.Application.Appointments.Commands.UpdateAppointment;

public class UpdateAppointmentValidator: AbstractValidator<UpdateAppointmentCommand>
{
    private readonly IApplicationDbContext _context;
    
    public UpdateAppointmentValidator(IApplicationDbContext context)
    {
        _context = context;
        RuleFor(x => x.WorkspaceId).NotEmpty();
        RuleFor(x => x.PatientId).NotEmpty();
        RuleFor(x => x.MedicalNotes)
            .NotEmpty()
            .WithMessage("Medical records are required");

        // validate if PatientTreatment exists and belongs to the patient
        RuleForEach(x => x.MedicalNotes)
            .MustAsync(PatientTreatmentBelongsToPatient)
            .WithMessage("Patient treatment does not exist or does not belong to the patient");
        
        // validate if the PatientTreatment status is not finished
        RuleForEach(x => x.MedicalNotes)
            .MustAsync(PatientTreatmentNotFinished)
            .WithMessage("Patient treatment is finished");
        
        // validate if the MedicalNote is new or belongs to the appointment
        RuleForEach(x => x.MedicalNotes)
            .MustAsync(MedicalNoteBelongsToAppointment)
            .WithMessage($"Medical record does not belong to the appointment");
        
        RuleForEach(command => command.MedicalNotes).SetValidator(new UpdateMeicalRecordDtoValidator());

        RuleFor(x => x.Date).NotEmpty();
    }
    
    public class UpdateMeicalRecordDtoValidator : AbstractValidator<UpdateMedicalNoteDto>
    {
        public UpdateMeicalRecordDtoValidator()
        {
            RuleFor(dto => dto.PatientTreatmentId).NotEmpty();
            RuleFor(dto => dto.Description).NotEmpty().MaximumLength(256);
            RuleFor(dto => dto.Observations).MaximumLength(256);
        }
    }

    private async Task<bool> MedicalNoteBelongsToAppointment(UpdateAppointmentCommand arg1, UpdateMedicalNoteDto arg2, CancellationToken arg3)
    {
        if (arg2.Id == 0) return true; // new medical record
        
        var medicalNote = await _context.MedicalNotes
            .Include(x => x.Appointment)
            .Where(x => x.Id == arg2.Id)
            .Where(x => x.AppointmentId == arg1.Id)
            .AsNoTracking()
            .FirstOrDefaultAsync(arg3);
        
        return medicalNote is not null;
    }

    private async Task<bool> PatientTreatmentNotFinished(UpdateMedicalNoteDto arg1, CancellationToken arg2)
    {
        var patientTreatment = await _context.PatientTreatments
            .Where(x => x.Id == arg1.PatientTreatmentId)
            .AsNoTracking()
            .FirstOrDefaultAsync(arg2);
        
        if (patientTreatment == null) return true; // already validated in PatientTreatmentBelongsToPatient

        return patientTreatment.Status != TreatmentStatus.Finished;
    }

    private async Task<bool> PatientTreatmentBelongsToPatient(UpdateAppointmentCommand arg1, UpdateMedicalNoteDto arg2, CancellationToken arg3)
    {
        var patientTreatment = await _context.PatientTreatments
            .AsNoTracking()
            .Include(x => x.Budget)
            .Where(x => x.Id == arg2.PatientTreatmentId)
            .Where(x => x.Budget.PatientId == arg1.PatientId)
            .FirstOrDefaultAsync(arg3);
        
        return patientTreatment is not null;
    }
}