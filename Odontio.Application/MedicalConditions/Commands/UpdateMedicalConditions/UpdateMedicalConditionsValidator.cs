using FluentValidation.Validators;
using Odontio.Application.Common.Interfaces;

namespace Odontio.Application.MedicalConditions.Common.UpdateMedicalConditions;

public class UpdateMedicalConditionsValidator : AbstractValidator<UpdateMedicalConditionsCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateMedicalConditionsValidator(IApplicationDbContext context)
    {
        _context = context;
        RuleFor(x => x.PatientId)
            .NotEmpty()
            .WithMessage("Patient id is required.");
        
        RuleFor(x => x.WorkspaceId)
            .NotEmpty()
            .WithMessage("Workspace id is required.");
        
        RuleFor(x => x.MedicalConditions)
            .NotEmpty()
            .WithMessage("Medical conditions are required.");

        RuleFor(x => x.MedicalConditions)
            .ForEach(x =>
                x.Must(mc => !string.IsNullOrEmpty(mc.ConditionType)).WithMessage("Is required"));

        // validate each conditiontype is unique in the list
        RuleFor(x => x.MedicalConditions)
            .ForEach(x =>
                x.Must((command, x) => command.Count(mc => mc.ConditionType == x.ConditionType) == 1)
                    .WithMessage("Repeated condition type."));
    }
}

public class AddConditionTypeValidator(IApplicationDbContext dbContext)
    : IAsyncPropertyValidator<UpdateMedicalConditionsCommand, MedicalConditionDto>
{
    public async Task<bool> IsValidAsync(ValidationContext<UpdateMedicalConditionsCommand> context,
        MedicalConditionDto value, CancellationToken cancellation)
    {
        // validata each conditiontype is unique for the patient
        var exists = await dbContext.MedicalConditions
            .AnyAsync(x => x.ConditionType.ToLower() == value.ConditionType.ToLower() &&
                           x.PatientId == context.InstanceToValidate.PatientId, cancellationToken: cancellation);
    
        if (exists)
        {
            context.MessageFormatter.AppendArgument("ErrorMessage", $"Condition type {value.ConditionType} already exists for this patient.");
            return false;
        }
        
        // validate each description is 256 max length
        if (value.Description?.Length > 256)
        {
            context.MessageFormatter.AppendArgument("ErrorMessage", $"Description must be less than 256 characters.");
            return false;
        }
    
        return true;
    }

    public string Name => nameof(AddConditionTypeValidator);

    public string GetDefaultMessageTemplate(string errorCode)
    {
        return "{ErrorMessage}";
    }
}