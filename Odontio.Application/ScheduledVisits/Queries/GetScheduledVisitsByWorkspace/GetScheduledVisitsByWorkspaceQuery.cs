using Odontio.Application.Common.Attributes;
using Odontio.Application.Common.Interfaces;

namespace Odontio.Application.ScheduledVisits.Queries.GetScheduledVisitsByWorkspace;

[ValidateWorkspace]
[RolesAuthorize(nameof(RolesEnum.Administrator))]
public class GetScheduledVisitsByWorkspaceQuery: IRequest<IEnumerable<GetScheduledVisitByWorkspaceResult>>, IWorkspaceResource
{
    public long WorkspaceId { get; set; }
}