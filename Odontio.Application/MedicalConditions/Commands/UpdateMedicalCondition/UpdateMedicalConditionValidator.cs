using Odontio.Application.Common.Interfaces;
using Odontio.Application.MedicalConditions.UpdateMedicalCondition;

namespace Odontio.Application.MedicalConditions.Common.UpdateMedicalCondition;

public class UpdateMedicalConditionValidator : AbstractValidator<UpdateMedicalConditionCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateMedicalConditionValidator(IApplicationDbContext context)
    {
        _context = context;
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Medical condition id is required.");

        RuleFor(x => x.WorkspaceId)
            .NotEmpty()
            .WithMessage("Workspace id is required.");

        RuleFor(x => x.PatientId)
            .NotEmpty()
            .WithMessage("Patient id is required.");
        
        RuleFor(x => x.ConditionType)
            .NotEmpty()
            .WithMessage("Condition type is required.")
            .MustAsync(BeUniqueConditionType)
            .WithMessage(x => $"Condition type {x.ConditionType} already exists for this patient.");

        RuleFor(x => x.Description)
            .MaximumLength(256)
            .WithMessage("Description must be less than 256 characters.");
    }

    private async Task<bool> BeUniqueConditionType(UpdateMedicalConditionCommand command, string conditionType,
        CancellationToken cancellationToken)
    {
        // validate if exists a condition type with the same name but different id for the same patient
        var exists = await _context.MedicalConditions
            .AsNoTracking()
            .AnyAsync(
            x => x.ConditionType.ToLower() == conditionType.ToLower() &&
                 x.PatientId == command.PatientId &&
                 x.Id != command.Id, cancellationToken);

        return !exists;
    }
}