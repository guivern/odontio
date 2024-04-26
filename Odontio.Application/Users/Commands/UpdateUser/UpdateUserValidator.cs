using Odontio.Application.Common.Interfaces;

namespace Odontio.Application.Users.Commands.UpdateUser;

public class UpdateUserValidator : AbstractValidator<UpdateUserCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateUserValidator(IApplicationDbContext context)
    {
        _context = context;
        RuleFor(x => x.Username).NotEmpty().MaximumLength(48).MustAsync(BeUniqueUsername)
            .WithMessage("Ya está en uso.");
        RuleFor(x => x.Email).MaximumLength(100).EmailAddress().MustAsync(BeUniqueEmail)
            .WithMessage("Ya está en uso.");
        RuleFor(x => x.FirstName).MaximumLength(48);
        RuleFor(x => x.LastName).MaximumLength(48);
        RuleFor(x => x.PhotoUrl).MaximumLength(256);
        RuleFor(x => x.RoleId).NotEmpty().MustAsync(RoleExists)
            .WithMessage("El rol no existe.");
        RuleFor(x => x.WorkspaceId).Must(x => x == null).When(x => x.RoleId == (long)RolesEnum.Administrator)
            .WithMessage("El rol 'Administrador' no puede tener un workspace asignado.");
        RuleFor(x => x.WorkspaceId).Must(x => x != null).When(x => x.RoleId != (long)RolesEnum.Administrator)
            .WithMessage("Es requerido.");
        RuleFor(x => x.WorkspaceId).MustAsync(WorkspaceExists)
            .When(x => x.RoleId != (long)RolesEnum.Administrator)
            .WithMessage("El workspace no existe.");
    }
    
    private Task<bool> WorkspaceExists(UpdateUserCommand arg1, long? arg2, CancellationToken arg3)
    {
        if (arg2 == null) return Task.FromResult(true);
        var exists = _context.Workspaces.AsNoTracking().AnyAsync(x => x.Id == arg2, arg3);
        
        return exists;
    }

    private async Task<bool> BeUniqueEmail(UpdateUserCommand arg1, string arg2, CancellationToken arg3)
    {
        if (string.IsNullOrEmpty(arg2)) return true;
        var exists = await _context.Users.AsNoTracking()
            .AnyAsync(x => x.Email != null && x.Email.ToLower() == arg2.ToLower() && x.Id != arg1.Id, arg3);
        
        return !exists;
    }

    private async Task<bool> BeUniqueUsername(UpdateUserCommand arg1, string arg2, CancellationToken arg3)
    {
        var exists = await _context.Users.AsNoTracking()
            .AnyAsync(x => x.Username.ToLower() == arg2.ToLower() && x.Id != arg1.Id, arg3);
        
        return !exists;
    }

    private async Task<bool> RoleExists(UpdateUserCommand arg1, long arg2, CancellationToken arg3)
    {
        return await _context.Roles.AsNoTracking().AnyAsync(x => x.Id == arg2, arg3);
    }
}