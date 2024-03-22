using Odontio.Application.Common.Attributes;
using Odontio.Application.Common.Interfaces;

namespace Odontio.Application.Profile.Commands.UpdateProfile;

[RolesAuthorize(nameof(RolesEnum.Administrator), nameof(RolesEnum.User))]
public class UpdateProfileCommand : IRequest<ErrorOr<UpdateProfileResult>>
{
    public long Id { get; set; }
    public string Username { get; set; } = null!;
    public string? Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? PhotoUrl { get; set; }
}