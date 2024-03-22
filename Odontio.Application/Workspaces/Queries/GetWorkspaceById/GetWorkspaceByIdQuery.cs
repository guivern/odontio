using Odontio.Application.Common.Attributes;
using Odontio.Application.Workspaces.Common;

namespace Odontio.Application.Workspaces.Queries.GetWorkspaceById;

[RolesAuthorize(nameof(RolesEnum.Administrator))]
public class GetWorkspaceByIdQuery : IRequest<ErrorOr<UpsertWorkspaceResult>>
{
    public long Id { get; set; }
}