using Odontio.Application.Common.Interfaces;

namespace Odontio.Application.Workspaces.Commands.UpdateWorkspace;

public class UpdateWorkspaceValidator : AbstractValidator<UpdateWorkspaceCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateWorkspaceValidator(IApplicationDbContext context)
    {
        _context = context;
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100).MustAsync(BeUniqueName)
            .WithMessage("The name is already in use.");
        RuleFor(x => x.Address).MaximumLength(256);
        RuleFor(x => x.PhoneNumber).MaximumLength(20);
        RuleFor(x => x.Email).MaximumLength(100).EmailAddress();
        RuleFor(x => x.Ruc).MaximumLength(20);
        RuleFor(x => x.ContactName).MaximumLength(48);
        RuleFor(x => x.ContactPhoneNumber).MaximumLength(48);
    }

    private async Task<bool> BeUniqueName(UpdateWorkspaceCommand arg1, string arg2, CancellationToken arg3)
    {
        // check if exits a workspace with the same name and different id
        var exists = await _context.Workspaces.AsNoTracking()
            .AnyAsync(x => x.Name.ToLower() == arg2.ToLower() && x.Id != arg1.Id, arg3);

        return !exists;
    }
}