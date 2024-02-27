using FluentValidation.Validators;
using Odontio.Application.Common.Interfaces;
using Odontio.Domain.Enums;

namespace Odontio.Application.Patients.Commands.CreatePatient;

public class CreatePatientValidator : AbstractValidator<CreatePatientCommand>
{
    private readonly IApplicationDbContext _context;

    public CreatePatientValidator(IApplicationDbContext context)
    {
        _context = context;
        RuleFor(x => x.WorkspaceId).NotEmpty().WithMessage("Workspace id is required");
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(248);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(248);
        RuleFor(x => x.Gender).NotEmpty().Must(BeValidGender).WithMessage("Invalid gender");
        RuleFor(x => x.MaritalStatus).Must(BeValidMaritalStatus).WithMessage("Invalid marital status");
        RuleFor(x => x.DocumentNumber).NotEmpty().MustAsync(BeUniqueDocumentNumber).WithMessage("Already exists");
        // RuleFor(x => x.MedicalConditions)
        //     .ForEach(x =>
        //         x.Must(mc => !string.IsNullOrEmpty(mc.ConditionType)).WithMessage("Is required"));
    }

    private bool BeValidMaritalStatus(string? arg)
    {
        var result = Enum.TryParse<MaritalStatus>(arg, out _);
        return string.IsNullOrEmpty(arg) || Enum.TryParse<MaritalStatus>(arg, out _);
    }

    private bool BeValidGender(string arg1)
    {
        return Enum.TryParse<Gender>(arg1, out _);
    }

    private async Task<bool> BeUniqueDocumentNumber(string arg1, CancellationToken arg2)
    {
        return !await _context.Patients.AnyAsync(x => x.DocumentNumber.ToLower() == arg1.ToLower(),
            cancellationToken: arg2);
    }
}