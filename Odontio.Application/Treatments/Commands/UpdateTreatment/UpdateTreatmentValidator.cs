using Odontio.Application.Common.Interfaces;

namespace Odontio.Application.Treatments.Commands.UpdateTreatment;

public class UpdateTreatmentValidator : AbstractValidator<UpdateTreatmentCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateTreatmentValidator(IApplicationDbContext context)
    {
        _context = context;
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Name).NotEmpty().MustAsync(BeUniqueName).WithMessage("Already exists");
        RuleFor(x => x.CategoryId).NotEmpty().WithMessage("Category is required").MustAsync(CategoryExists)
            .WithMessage($"Category does not exist");
    }

    private async Task<bool> BeUniqueName(UpdateTreatmentCommand arg1, string arg2, CancellationToken arg3)
    {
        return !await _context.Treatments.AnyAsync(x => x.Name.ToLower() == arg2.ToLower() && arg1.Id != x.Id,
            cancellationToken: arg3);
    }

    private async Task<bool> CategoryExists(long arg1, CancellationToken arg2)
    {
        return await _context.Categories.AnyAsync(x => x.Id == arg1, cancellationToken: arg2);
    }
}