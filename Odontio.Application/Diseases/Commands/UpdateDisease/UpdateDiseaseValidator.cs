using Odontio.Application.Common.Interfaces;

namespace Odontio.Application.Diseases.Commands.UpdateDisease;

public class UpdateDiseaseValidator : AbstractValidator<UpdateDiseaseCommand>
{
    private readonly IApplicationDbContext _context;
    public UpdateDiseaseValidator(IApplicationDbContext context)
    {
        _context = context;
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200)
            .MustAsync(BeUniqueName).WithMessage("Ya existe una enfermedad con el mismo nombre");
    }

    private async Task<bool> BeUniqueName(UpdateDiseaseCommand arg1, string arg2, CancellationToken arg3)
    {
        return !await _context.Diseases
            .AsNoTracking()
            .Where(x => x.Name.ToLower() == arg2.ToLower())
            .Where(x => x.WorkspaceId == arg1.WorkspaceId)
            .Where(x => x.Id != arg1.Id)
            .AnyAsync(arg3);
    }
}