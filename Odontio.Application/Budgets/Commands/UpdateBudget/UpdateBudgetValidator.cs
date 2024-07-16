using FluentValidation.Validators;
using Odontio.Application.Common.Interfaces;

namespace Odontio.Application.Budgets.Commands.UpdateBudget;

public class UpdateBudgetValidator : AbstractValidator<UpdateBudgetCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateBudgetValidator(IApplicationDbContext context)
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
        // RuleFor(x => x.PatientTreatments)
        //     .ForEach(x => x.MustAsync(ToothExists).WithMessage("Tooth does not exist"));

        // validate that each cost is a positive value
        RuleFor(x => x.PatientTreatments)
            .ForEach(x => x.Must(y => y.Cost >= 0).WithMessage("Cost must be a positive value"));

        // validate that each patientTreatment is belongs to the budget
        RuleForEach(x => x.PatientTreatments)
            .SetAsyncValidator(new PatientTreatmentBelongsToBudgetValidator(_context));
    }


    // private Task<bool> ToothExists(UpdatePatientTreatment arg1, CancellationToken arg2)
    // {
    //     if (arg1.ToothId == null)
    //     {
    //         return Task.FromResult(true);
    //     }
    //
    //     var exists = _context.Teeth.AsNoTracking().AnyAsync(x => x.Id == arg1.ToothId, cancellationToken: arg2);
    //     return exists;
    // }

    private Task<bool> TreatmentExists(UpdatePatientTreatment arg1, CancellationToken arg2)
    {
        var exists = _context.Treatments.AsNoTracking()
            .AnyAsync(x => x.Id == arg1.TreatmentId, cancellationToken: arg2);
        return exists;
    }
}

public class
    PatientTreatmentBelongsToBudgetValidator(IApplicationDbContext dbContext)
    : IAsyncPropertyValidator<UpdateBudgetCommand, UpdatePatientTreatment>
{
    public async Task<bool> IsValidAsync(ValidationContext<UpdateBudgetCommand> context, UpdatePatientTreatment value,
        CancellationToken cancellation)
    {
        if (value.Id == 0) return true; // new patientTreatment

        var exists = await dbContext.PatientTreatments
            .AsNoTracking()
            .AnyAsync(x => x.Id == value.Id && x.BudgetId == context.InstanceToValidate.Id, cancellation);

        if (!exists)
        {
            context.MessageFormatter.AppendArgument("ErrorMessage",
                $"PatientTreatment with Id {value.Id} not found or does not belong to the budget with Id {context.InstanceToValidate.Id}");
        }

        return exists;
    }

    public string Name => nameof(PatientTreatmentBelongsToBudgetValidator);


    public string GetDefaultMessageTemplate(string errorCode)
    {
        return "{ErrorMessage}";
    }
}