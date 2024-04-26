using Odontio.Application.Common.Interfaces;

namespace Odontio.Application.Users.Commands.CreateUser;

public class CreateUserValidator : AbstractValidator<CreateUserCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateUserValidator(IApplicationDbContext context)
    {
        _context = context;
        RuleFor(x => x.Username).NotEmpty().MaximumLength(48).MustAsync(BeUniqueUsername)
            .WithMessage("Ya está en uso");
        RuleFor(x => x.Password)
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$")
            .WithMessage("El password debe tener al menos 8 caracteres, una mayúscula, una minúscula y un número.");
        RuleFor(x => x.ConfirmPassword).Equal(x => x.Password)
            .WithMessage("Las contraseñas no coinciden.");
        RuleFor(x => x.Email).MaximumLength(100).EmailAddress().MustAsync(BeUniqueEmail)
            .WithMessage("Ya está en uso");
        RuleFor(x => x.FirstName).MaximumLength(48);
        RuleFor(x => x.LastName).MaximumLength(48);
        RuleFor(x => x.PhotoUrl).MaximumLength(256);
        RuleFor(x => x.RoleId).NotEmpty()
            .MustAsync(RoleExists)
            .WithMessage("El rol no existe.");
        RuleFor(x => x.WorkspaceId).Must(x => x == null).When(x => x.RoleId == (long)RolesEnum.Administrator)
            .WithMessage("El rol 'Administrador' no puede tener un workspace asignado.");
        RuleFor(x => x.WorkspaceId).Must(x => x != null).When(x => x.RoleId != (long)RolesEnum.Administrator)
            .WithMessage("Es requerido.");
        RuleFor(x => x.WorkspaceId).MustAsync(WorkspaceExists)
            .When(x => x.RoleId != (long)RolesEnum.Administrator)
            .WithMessage("El workspace no existe.");
    }

    private Task<bool> WorkspaceExists(CreateUserCommand arg1, long? arg2, CancellationToken arg3)
    {
        if (arg2 == null) return Task.FromResult(true);
        var exists = _context.Workspaces.AsNoTracking().AnyAsync(x => x.Id == arg2, arg3);
        
        return exists;
    }

    private async Task<bool> RoleExists(CreateUserCommand arg1, long arg2, CancellationToken arg3)
    {
        return await _context.Roles.AsNoTracking().AnyAsync(x => x.Id == arg2, arg3);
    }

    private async Task<bool> BeUniqueEmail(CreateUserCommand arg1, string arg2, CancellationToken arg3)
    {
        if (string.IsNullOrEmpty(arg2)) return true;
        return !await _context.Users.AsNoTracking()
            .AnyAsync(x => x.Email != null && x.Email.ToLower() == arg2.ToLower(), arg3);
    }

    private async Task<bool> BeUniqueUsername(CreateUserCommand arg1, string arg2, CancellationToken arg3)
    {
        return await _context.Users.AsNoTracking()
            .AllAsync(x => x.Username.ToLower() != arg2.ToLower(), arg3);
    }
}