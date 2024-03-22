using Odontio.Application.Common.Interfaces;

namespace Odontio.Application.Profile.Commands.UpdateProfile;

public class UpdateProfileValidator : AbstractValidator<UpdateProfileCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateProfileValidator(IApplicationDbContext context)
    {
        _context = context;
        RuleFor(x => x.Username).NotEmpty().MaximumLength(48).MustAsync(BeUniqueUsername)
            .WithMessage("The username is already in use.");
        RuleFor(x => x.Email).MaximumLength(100).EmailAddress().MustAsync(BeUniqueEmail);
        RuleFor(x => x.FirstName).MaximumLength(48);
        RuleFor(x => x.LastName).MaximumLength(48);
        RuleFor(x => x.PhotoUrl).MaximumLength(256);
    }

    private async Task<bool> RoleExists(UpdateProfileCommand arg1, long arg2, CancellationToken arg3)
    {
        return await _context.Roles.AsNoTracking().AnyAsync(x => x.Id == arg2, arg3);
    }

    private async Task<bool> BeUniqueEmail(UpdateProfileCommand arg1, string arg2, CancellationToken arg3)
    {
        if (string.IsNullOrEmpty(arg2)) return true;
        var exists = await _context.Users.AsNoTracking()
            .AnyAsync(x => x.Email != null && x.Email.ToLower() == arg2.ToLower() && x.Id != arg1.Id, arg3);
        
        return !exists;
    }

    private async Task<bool> BeUniqueUsername(UpdateProfileCommand arg1, string arg2, CancellationToken arg3)
    {
        var exists = await _context.Users.AsNoTracking()
            .AnyAsync(x => x.Username.ToLower() == arg2.ToLower() && x.Id != arg1.Id, arg3);
        
        return !exists;
    }
}