using Odontio.Application.Common.Interfaces;

namespace Odontio.Application.Treatments.Commands.UpdateTreatment;

public class UpdateTreatmentValidator : AbstractValidator<UpdateTreatmentCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateTreatmentValidator(IApplicationDbContext context)
    {
        _context = context;
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.WorkspaceId).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MustAsync(BeUniqueName).WithMessage("Ya existe un tratamiento con el mismo nombre").MaximumLength(100);
        RuleFor(x => x.CategoryId).NotEmpty().WithMessage("Es requerido").MustAsync(CategoryExists)
            .WithMessage($"La categoría no existe");
    }

    private async Task<bool> BeUniqueName(UpdateTreatmentCommand arg1, string arg2, CancellationToken arg3)
    {
        return !await _context.Treatments
            .AsNoTracking()
            .Where(x => x.Name.ToLower() == arg2.ToLower())
            .Where(x => x.WorkspaceId == arg1.WorkspaceId)
            .Where(x => x.Id != arg1.Id)
            .AnyAsync(arg3);
    }

    private async Task<bool> CategoryExists(long arg1, CancellationToken arg2)
    {
        return await _context.Categories.AsNoTracking().AnyAsync(x => x.Id == arg1, cancellationToken: arg2);
    }
}