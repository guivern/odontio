using Odontio.Application.Common.Attributes;
using Odontio.Application.Common.Interfaces;

namespace Odontio.Application.Diseases.Commands.DeleteDisease;

[ValidateWorkspace]
[RolesAuthorize(nameof(RolesEnum.Administrator), nameof(RolesEnum.User))]
public class DeleteDiseaseCommand : IRequest<ErrorOr<Unit>>, IWorkspaceResource
{
    public long WorkspaceId { get; set; }
    public long Id { get; set; }
}