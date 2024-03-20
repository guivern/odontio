using Odontio.Application.Common.Interfaces;
using Odontio.Domain.Enums;

namespace Odontio.Application.MedicalRecords.Commands.CreateMedicalRecord;

public class CreateMedicalRecordValidator : AbstractValidator<CreateMedicalRecordCommand>
{
    private readonly IApplicationDbContext _context;
    
    public CreateMedicalRecordValidator(IApplicationDbContext context)
    {
        _context = context;
        RuleFor(x => x.Description).NotEmpty().MaximumLength(256);
        RuleFor(x => x.Observations).MaximumLength(256);
        
        // validate if PatientTreatment exists and belongs to the patient
        RuleFor(x => x.PatientTreatmentId)
            .MustAsync(PatientTreatmentExits)
            .WithMessage("Patient treatment not found");
        
        // validate if the PatientTreatment status is not finished
        RuleFor(x => x.PatientTreatmentId)
            .MustAsync(PatientTreatmentNotFinished)
            .WithMessage("Patient treatment is finished");
    }

    private async Task<bool> PatientTreatmentNotFinished(CreateMedicalRecordCommand arg1, long arg2, CancellationToken arg3)
    {
        var patientTreatment = await _context.PatientTreatments
            .Where(x => x.Id == arg2)
            .AsNoTracking()
            .FirstOrDefaultAsync(arg3);
        
        if (patientTreatment == null) return true; // patient treatment not found
        
        return patientTreatment.Status != TreatmentStatus.Finished;
    }

    private async Task<bool> PatientTreatmentExits(CreateMedicalRecordCommand arg1, long arg2, CancellationToken arg3)
    {
        var exits = await _context.PatientTreatments
            .Include(x => x.Budget)
            .Where(x => x.Id == arg2)
            .Where(x => x.Budget.PatientId == arg1.PatientId)
            .AsNoTracking()
            .AnyAsync(arg3);
        
        return exits;
    }
}