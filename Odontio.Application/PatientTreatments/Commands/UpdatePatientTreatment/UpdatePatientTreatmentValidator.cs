using Odontio.Application.Common.Interfaces;

namespace Odontio.Application.PatientTreatments.Commands.UpdatePatientTreatment;

public class UpdatePatientTreatmentValidator : AbstractValidator<UpdatePatientTreatmentCommand>
{
    private readonly IApplicationDbContext _context;
    
    public UpdatePatientTreatmentValidator(IApplicationDbContext context)
    {
        _context = context;
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.WorkspaceId).NotEmpty();
        RuleFor(x => x.PatientId).NotEmpty();
        RuleFor(x => x.BudgetId).NotEmpty()
            .MustAsync(BudgetBelongsToPatient).WithMessage("Budget not found");
        RuleFor(x => x.TreatmentId).NotEmpty()
            .MustAsync(TreatmentExists).WithMessage("Treatment not found");
        RuleFor(x => x.Cost).NotEmpty()
            .Must(x => x >= 0).WithMessage("Cost must be greater than or equal to 0");
    }

    private async Task<bool> TreatmentExists(UpdatePatientTreatmentCommand arg1, long arg2, CancellationToken arg3)
    {
        var treatment = await _context.Treatments.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == arg2, arg3);
        
        return treatment != null;
    }

    private async Task<bool> BudgetBelongsToPatient(UpdatePatientTreatmentCommand arg1, long arg2, CancellationToken arg3)
    {
        var budget = await _context.Budgets.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == arg2 && x.PatientId == arg1.PatientId, arg3);
        
        return budget != null;
    }
}