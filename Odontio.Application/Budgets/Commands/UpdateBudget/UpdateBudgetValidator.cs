using FluentValidation.Validators;
using Odontio.Application.Common.Interfaces;

namespace Odontio.Application.Budgets.Commands.UpdateBudget;

public class UpdateBudgetValidator : AbstractValidator<UpdateBudgetCommand>
{
    private readonly IApplicationDbContext _context;
    public UpdateBudgetValidator(IApplicationDbContext  context)
    {
        _context = context;
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Date).NotEmpty();
        RuleFor(x => x.WorkspaceId).NotEmpty();
        RuleFor(x => x.PatientId).NotEmpty();
        
        // validate that PatientTreatment is not null
        RuleFor(x => x.PatientTreatments)
            .NotEmpty()
            .WithMessage("PatientTreatments are required");

        // validate that each treatmentId exists
        RuleFor(x => x.PatientTreatments)
            .ForEach(x => x.MustAsync(TreatmentExists).WithMessage("Treatment does not exist"));
        
        // validate that each toothId exists
        RuleFor(x => x.PatientTreatments)
            .ForEach(x => x.MustAsync(ToothExists).WithMessage("Tooth does not exist"));
        
        // validate that each cost is a positive value
        RuleFor(x => x.PatientTreatments)
            .ForEach(x => x.Must(y => y.Cost >= 0).WithMessage("Cost must be a positive value"));
    }

    private Task<bool> ToothExists(UpdatePatientTreatment arg1, CancellationToken arg2)
    {
        if (arg1.ToothId == null)
        {
            return Task.FromResult(true);
        }
        
        var exists = _context.Teeth.AnyAsync(x => x.Id == arg1.ToothId);
        return exists;
    }

    private Task<bool> TreatmentExists(UpdatePatientTreatment arg1, CancellationToken arg2)
    {
        var exists = _context.Treatments.AnyAsync(x => x.Id == arg1.TreatmentId);
        return exists;
    }
}