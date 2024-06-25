using Odontio.Application.Common.Interfaces;

namespace Odontio.Application.MedicalConditionQuestions.Commands.CreateMedicalConditionQuestion;

public class CreateMedicalConditionQuestionValidator : AbstractValidator<CreateMedicalConditionQuestionCommand>
{
    private readonly IApplicationDbContext _context;
    
    public CreateMedicalConditionQuestionValidator(IApplicationDbContext context)
    {
        _context = context;
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200)
            .MustAsync(BeUniqueName).WithMessage("Ya existe una pregunta con este nombre.");
    }

    private async Task<bool> BeUniqueName(CreateMedicalConditionQuestionCommand arg1, string arg2, CancellationToken arg3)
    {
        return !await _context.MedicalConditionQuestions
            .AsNoTracking()
            .Where(x => x.Name.ToLower() == arg2.ToLower())
            .Where(x => x.WorkspaceId == arg1.WorkspaceId)
            .AnyAsync(arg3);
    }
}