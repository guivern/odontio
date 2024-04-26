using Odontio.Application.Common.Attributes;

namespace Odontio.Application.Users.Commands.ToggleActive;

[RolesAuthorize(nameof(RolesEnum.Administrator))]
public class ToggleActiveCommand : IRequest<ErrorOr<ToggleActiveResult>>
{
    public long Id { get; set; }
}