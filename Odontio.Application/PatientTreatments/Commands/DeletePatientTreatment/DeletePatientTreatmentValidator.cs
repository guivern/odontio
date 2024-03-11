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
        RuleFor(x => x.BudgetId).NotEmpty()
            .MustAsync(BudgetBelongsToPatient).WithMessage("Budget not found");
    }

    private async Task<bool> BudgetBelongsToPatient(DeletePatientTreatmentCommand arg1, long arg2, CancellationToken arg3)
    {
        var budget = await _context.Budgets.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == arg2 && x.PatientId == arg1.PatientId, arg3);
        
        return budget != null;
    }
}