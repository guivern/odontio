using Odontio.Application.Common.Interfaces;
using Odontio.Domain.Enums;

namespace Odontio.Application.Appointments.Commands.CreateAppointment;

public class CreateAppointmentValidator : AbstractValidator<CreateAppointmentCommand>
{
    private readonly IApplicationDbContext _context;
    
    public CreateAppointmentValidator(IApplicationDbContext context)
    {
        _context = context;
        RuleFor(x => x.MedicalRecords)
            .NotEmpty()
            .WithMessage("Medical records are required");

        // validate if PatientTreatment exists and belongs to the patient
        RuleForEach(x => x.MedicalRecords)
            .MustAsync(PatientTreatmentExits)
            .WithMessage("Patient treatment not found");
        
        // validate if the PatientTreatment status is not finished
        RuleForEach(x => x.MedicalRecords)
            .MustAsync(PatientTreatmentNotFinished)
            .WithMessage("Patient treatment is finished");
        
        // validate that Description is required  and has a maximum length of 256 for each medical record
        RuleForEach(command => command.MedicalRecords).SetValidator(new CreateMedicalRecordDtoValidator());
    }
    
    public class CreateMedicalRecordDtoValidator : AbstractValidator<CreateMedicalRecordDto>
    {
        public CreateMedicalRecordDtoValidator()
        {
            RuleFor(dto => dto.PatientTreatmentId).NotEmpty();
            RuleFor(dto => dto.Description).NotEmpty().MaximumLength(256);
            RuleFor(dto => dto.Observations).MaximumLength(256);
        }
    }

    private async Task<bool> PatientTreatmentNotFinished(CreateMedicalRecordDto arg1, CancellationToken arg2)
    {
        var patientTreatment = await _context.PatientTreatments
            .Where(x => x.Id == arg1.PatientTreatmentId)
            .AsNoTracking()
            .FirstOrDefaultAsync(arg2);
        
        if (patientTreatment == null) return true; // patient treatment not found
        
        return patientTreatment.Status != TreatmentStatus.Finished;
    }

    private async Task<bool> PatientTreatmentExits(CreateAppointmentCommand arg1, CreateMedicalRecordDto arg2, CancellationToken arg3)
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