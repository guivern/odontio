using Odontio.Application.Common.Interfaces;

namespace Odontio.Application.Diseases.Commands.CreateDisease;

public class CreateDiseaseValidator : AbstractValidator<CreateDiseaseCommand>
{
    private readonly IApplicationDbContext _context;
    public CreateDiseaseValidator(IApplicationDbContext context)
    {
        _context = context;
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200)
            .MustAsync(BeUniqueName).WithMessage("The specified name already exists");
    }

    private async Task<bool> BeUniqueName(CreateDiseaseCommand arg1, string arg2, CancellationToken arg3)
    {
        return !await _context.Diseases
            .AsNoTracking()
            .Where(x => x.Name.ToLower() == arg2.ToLower())
            .Where(x => x.WorkspaceId == arg1.WorkspaceId)
            .AnyAsync(arg3);
    }
}