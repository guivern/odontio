using Odontio.Application.Common.Interfaces;

namespace Odontio.Application.MedicalNotes.Commands.DeleteMedicalNote;

public class DeleteMedicalNoteValidator : AbstractValidator<DeleteMedicalNoteCommand>
{
    private readonly IApplicationDbContext _context;
    
    public DeleteMedicalNoteValidator(IApplicationDbContext context)
    {
        _context = context;
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.WorkspaceId).NotEmpty();
        RuleFor(x => x.PatientId).NotEmpty();
        RuleFor(x => x.AppointmentId).NotEmpty();
    }
}