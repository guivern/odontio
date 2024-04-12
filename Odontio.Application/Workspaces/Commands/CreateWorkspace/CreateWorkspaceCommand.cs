using Odontio.Application.Common.Attributes;
using Odontio.Application.Workspaces.Common;

namespace Odontio.Application.Workspaces.Commands.CreateWorkspace;

[RolesAuthorize(nameof(RolesEnum.Administrator))]
public class CreateWorkspaceCommand : IRequest<ErrorOr<UpsertWorkspaceResult>>
{
    public string Name { get; set; } = null!;
    public string? Address { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? Ruc { get; set; }
    public string? ContactName { get; set; }
    public string? ContactPhoneNumber { get; set; }
    public string? BusinessName { get; set; }
}