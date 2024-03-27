using Odontio.Application.Common.Interfaces;

namespace Odontio.Application.MedicalNotes.Commands.UpdateMedicalNote;

public class UpdateMedicalNoteValidator : AbstractValidator<UpdateMedicalNoteCommand>
{
    private readonly IApplicationDbContext _context;
    
    public UpdateMedicalNoteValidator(IApplicationDbContext context)
    {
        _context = context;
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.WorkspaceId).NotEmpty();
        RuleFor(x => x.PatientId).NotEmpty();
        RuleFor(x => x.AppointmentId).NotEmpty();
        RuleFor(x => x.PatientTreatmentId).NotEmpty()
            .MustAsync(PatientTreatmentBelongsToPatient).WithMessage("Patient treatment not found");
        RuleFor(x => x.Description).NotEmpty().MaximumLength(256);
        RuleFor(x => x.Observations).MaximumLength(256);
    }

    private async Task<bool> PatientTreatmentBelongsToPatient(UpdateMedicalNoteCommand arg1, long arg2, CancellationToken arg3)
    {
        var patientTreatment = await _context.PatientTreatments
            .Include(x => x.Budget)
            .Where(x => x.Id == arg2)
            .Where(x => x.Budget.PatientId == arg1.PatientId)
            .AsNoTracking()
            .FirstOrDefaultAsync(arg3);
        
        return patientTreatment is not null;
    }
}