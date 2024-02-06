﻿using Odontio.Application.Common.Interfaces;

namespace Odontio.Application.Treatments.Common.CreateTreatment;

public class CreateTreatmentValidator: AbstractValidator<CreateTreatmentCommand>
{
    private readonly IApplicationDbContext _context;
    
    public CreateTreatmentValidator(IApplicationDbContext context)
    {
        _context = context;
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
        RuleFor(x => x.CategoryId).NotEmpty().WithMessage("Category is required").MustAsync(CategoryExists)
            .WithMessage($"Category does not exist");
    }

    private async Task<bool> CategoryExists(long arg1, CancellationToken arg2)
    {
        return await _context.Categories.AnyAsync(x => x.Id == arg1, cancellationToken: arg2);
    }
}