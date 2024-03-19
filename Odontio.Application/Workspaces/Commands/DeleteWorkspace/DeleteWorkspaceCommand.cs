using Odontio.Application.Common.Attributes;

namespace Odontio.Application.Workspaces.Commands.DeleteWorkspace;

[RolesAuthorize(nameof(RolesEnum.Administrator))]
public class DeleteWorkspaceCommand : IRequest<ErrorOr<Unit>>
{
    public long Id { get; set; }
}