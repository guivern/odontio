using Odontio.Application.Workspaces.Common;

namespace Odontio.Application.Workspaces.Queries.GetWorkspaceById;

public class GetWorkspaceByIdQuery : IRequest<ErrorOr<UpsertWorkspaceResult>>
{
    public long Id { get; set; }
}