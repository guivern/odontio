﻿using Odontio.Application.Common.Interfaces;

namespace Odontio.Application.Workspaces.Commands.CreateWorkspace;

public class CreateWorkspaceValidator : AbstractValidator<CreateWorkspaceCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateWorkspaceValidator(IApplicationDbContext context)
    {
        _context = context;
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100).MustAsync(BeUniqueName)
            .WithMessage("El nombre ya está en uso.");
        RuleFor(x => x.BusinessName).MaximumLength(100);
        RuleFor(x => x.Address).MaximumLength(256);
        RuleFor(x => x.PhoneNumber).MaximumLength(20);
        RuleFor(x => x.Email).MaximumLength(100).EmailAddress();
        RuleFor(x => x.Ruc).MaximumLength(20);
        RuleFor(x => x.ContactName).MaximumLength(48);
        RuleFor(x => x.ContactPhoneNumber).MaximumLength(48);
    }

    private async Task<bool> BeUniqueName(CreateWorkspaceCommand arg1, string? arg2, CancellationToken arg3)
    {
        if (arg2 is null) return true;
         return await _context.Workspaces.AsNoTracking()
            .AllAsync(x => x.Name.ToLower() != arg2.ToLower(), arg3);
    }
}