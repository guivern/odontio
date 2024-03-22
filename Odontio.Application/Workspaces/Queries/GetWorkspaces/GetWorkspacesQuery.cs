using Odontio.Application.Common.Helpers;
using Odontio.Application.Workspaces.Common;

namespace Odontio.Application.Workspaces.Queries.GetWorkspaces;

public class GetWorkspacesQuery : PagedListQueryBase, IRequest<ErrorOr<PagedList<UpsertWorkspaceResult>>>
{
}