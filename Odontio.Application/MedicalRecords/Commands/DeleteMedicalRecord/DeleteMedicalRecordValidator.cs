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
        RuleFor(x => x.AppointmentId).NotEmpty();
    }
}