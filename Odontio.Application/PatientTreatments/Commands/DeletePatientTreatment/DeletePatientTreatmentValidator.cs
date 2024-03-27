using Odontio.Application.Common.Interfaces;

namespace Odontio.Application.PatientTreatments.Commands.DeletePatientTreatment;

public class DeletePatientTreatmentValidator : AbstractValidator<DeletePatientTreatmentCommand>
{
    private readonly IApplicationDbContext _context;
    
    public DeletePatientTreatmentValidator(IApplicationDbContext context)
    {
        _context = context;
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.WorkspaceId).NotEmpty();
        RuleFor(x => x.PatientId).NotEmpty();
        RuleFor(x => x.BudgetId).NotEmpty();
    }
}