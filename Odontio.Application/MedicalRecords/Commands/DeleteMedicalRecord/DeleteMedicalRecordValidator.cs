using Odontio.Application.Common.Interfaces;

namespace Odontio.Application.MedicalRecords.Commands.DeleteMedicalRecord;

public class DeleteMedicalRecordValidator : AbstractValidator<DeleteMedicalRecordCommand>
{
    private readonly IApplicationDbContext _context;
    
    public DeleteMedicalRecordValidator(IApplicationDbContext context)
    {
        _context = context;
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.WorkspaceId).NotEmpty();
        RuleFor(x => x.PatientId).NotEmpty();
        RuleFor(x => x.AppointmentId).NotEmpty()
            .MustAsync(AppointmentBelongsToPatient).WithMessage("Appointment not found");
    }

    private async Task<bool> AppointmentBelongsToPatient(DeleteMedicalRecordCommand arg1, long arg2, CancellationToken arg3)
    {
        var appointment = await _context.Appointments.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == arg2 && x.PatientId == arg1.PatientId, arg3);
        
        return appointment != null;
    }
}