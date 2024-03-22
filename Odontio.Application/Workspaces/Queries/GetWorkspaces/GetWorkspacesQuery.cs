using Odontio.Application.Common.Attributes;
using Odontio.Application.Common.Helpers;
using Odontio.Application.Workspaces.Common;

namespace Odontio.Application.Workspaces.Queries.GetWorkspaces;

[RolesAuthorize(nameof(RolesEnum.Administrator))]
public class GetWorkspacesQuery : PagedListQueryBase, IRequest<ErrorOr<PagedList<UpsertWorkspaceResult>>>
{
}