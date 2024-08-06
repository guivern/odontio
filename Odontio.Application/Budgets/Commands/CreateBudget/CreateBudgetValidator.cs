using FluentValidation.Validators;
using Odontio.Application.Common.Interfaces;

namespace Odontio.Application.Budgets.Commands.CreateBudget;

public class CreateBudgetValidator : AbstractValidator<CreateBudgetCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateBudgetValidator(IApplicationDbContext context)
    {
        _context = context;
        RuleFor(x => x.WorkspaceId)
            .NotEmpty()
            .WithMessage("Workspace is required");

        RuleFor(x => x.PatientId)
            .NotEmpty()
            .WithMessage("Patient is required");

        // validate that PatientTreatment is not null
        RuleFor(x => x.Details)
            .NotEmpty()
            .WithMessage("PatientTreatments are required");

        // cost must be a positive value, toothId and treatmentId must exist
        // RuleForEach(x => x.PatientTreatments).SetAsyncValidator(new AddPatientTreatmentValidator(_context));

        // validate that each treatmentId exists
        RuleFor(x => x.Details)
            .ForEach(x => x.MustAsync(TreatmentExists).WithMessage("Treatment does not exist"));
        
        // validate that each toothId exists
        // RuleFor(x => x.PatientTreatments)
        //     .ForEach(x => x.MustAsync(ToothExists).WithMessage("Tooth does not exist"));
        
        // validate that each cost is a positive value
        RuleFor(x => x.Details)
            .ForEach(x => x.Must(y => y.Cost >= 0).WithMessage("Cost must be a positive value"));
    }

    // private Task<bool> ToothExists(CreatePatientTreatmentDto arg1, CancellationToken arg2)
    // {
    //     if (arg1.ToothId == null)
    //     {
    //         return Task.FromResult(true);
    //     }
    //     
    //     var exists = _context.Teeth.AsNoTracking().AnyAsync(x => x.Id == arg1.ToothId, cancellationToken: arg2);
    //     return exists;
    // }

    private Task<bool> TreatmentExists(CreatePatientTreatmentDto arg1, CancellationToken arg2)
    {
        var exists = _context.Treatments.AsNoTracking().AnyAsync(x => x.Id == arg1.Treatment.Id, cancellationToken: arg2);
        return exists;
    }
}