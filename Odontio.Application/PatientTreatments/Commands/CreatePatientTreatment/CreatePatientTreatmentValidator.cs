using Odontio.Application.Common.Interfaces;

namespace Odontio.Application.PatientTreatments.Commands.CreatePatientTreatment;

public class CreatePatientTreatmentValidator : AbstractValidator<CreatePatientTreatmentCommand>
{
    private readonly IApplicationDbContext _context;
    
    public CreatePatientTreatmentValidator(IApplicationDbContext context)
    {
        _context = context;
        RuleFor(x => x.WorkspaceId).NotEmpty();
        RuleFor(x => x.PatientId).NotEmpty();
        RuleFor(x => x.BudgetId).NotEmpty();
        RuleFor(x => x.TreatmentId).NotEmpty()
            .MustAsync(TreatmentExists).WithMessage("Treatment not found");
        RuleFor(x => x.Cost)
            .Must(x => x >= 0).WithMessage("Cost must be greater than or equal to 0");
        RuleFor(x => x.ToothId).MustAsync(ToothExists).WithMessage("Tooth not found");
    }

    private async Task<bool> ToothExists(long? arg1, CancellationToken arg2)
    {
        if (arg1 == null)
            return true;
        
        var tooth = await _context.Teeth.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == arg1, arg2);
        
        return tooth != null;
    }

    private async Task<bool> TreatmentExists(long arg1, CancellationToken arg2)
    {
        var treatment = await _context.Treatments.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == arg1, arg2);
        
        return treatment != null;
    }
}