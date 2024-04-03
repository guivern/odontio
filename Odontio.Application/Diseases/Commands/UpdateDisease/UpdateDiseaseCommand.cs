using Odontio.Application.Common.Attributes;
using Odontio.Application.Common.Interfaces;
using Odontio.Application.Diseases.Common;

namespace Odontio.Application.Diseases.Commands.UpdateDisease;

[ValidateWorkspace]
[RolesAuthorize(nameof(RolesEnum.Administrator), nameof(RolesEnum.User))]
public class UpdateDiseaseCommand: IRequest<ErrorOr<UpsertDiseaseResult>>, IWorkspaceResource
{
    public long WorkspaceId { get; set; }
    public long Id { get; set; }
    public string Name { get; set; } = null!;
}