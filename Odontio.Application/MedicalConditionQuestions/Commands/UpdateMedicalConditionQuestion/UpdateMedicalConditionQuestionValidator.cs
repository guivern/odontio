using Odontio.Application.Common.Interfaces;

namespace Odontio.Application.MedicalConditionQuestions.Commands.UpdateMedicalConditionQuestion;

public class UpdateMedicalConditionQuestionValidator : AbstractValidator<UpdateMedicalConditionQuestionCommand>
{
    private readonly IApplicationDbContext _context;
    public UpdateMedicalConditionQuestionValidator(IApplicationDbContext context)
    {
        _context = context;
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200)
            .MustAsync(BeUniqueName).WithMessage("The specified name already exists");
    }

    private async Task<bool> BeUniqueName(UpdateMedicalConditionQuestionCommand arg1, string arg2, CancellationToken arg3)
    {
        return !await _context.Diseases
            .AsNoTracking()
            .Where(x => x.Name.ToLower() == arg2.ToLower())
            .Where(x => x.WorkspaceId == arg1.WorkspaceId)
            .Where(x => x.Id != arg1.Id)
            .AnyAsync(arg3);
    }
}