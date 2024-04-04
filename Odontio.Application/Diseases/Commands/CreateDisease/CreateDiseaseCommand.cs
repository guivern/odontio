using Odontio.Application.Common.Attributes;
using Odontio.Application.Common.Interfaces;
using Odontio.Application.Diseases.Common;

namespace Odontio.Application.Diseases.Commands.CreateDisease;

[ValidateWorkspace]
[RolesAuthorize(nameof(RolesEnum.Administrator), nameof(RolesEnum.User))]
public class CreateDiseaseCommand : IRequest<ErrorOr<UpsertDiseaseResult>>, IWorkspaceResource
{
    public long WorkspaceId { get; set; }
    public string Name { get; set; } = null!;
}