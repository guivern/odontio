using Odontio.Application.Common.Attributes;
using Odontio.Application.Common.Helpers;
using Odontio.Application.Common.Interfaces;
using Odontio.Application.ScheduledVisits.Common;
using Odontio.Domain.Enums;

namespace Odontio.Application.ScheduledVisits.Queries.GetScheduledVisitsByWorkspace;

[ValidateWorkspace]
[RolesAuthorize(nameof(Roles.Administrator))]
public class GetScheduledVisitsByWorkspaceQuery: IRequest<IEnumerable<UpsertScheduledVisitResult>>, IWorkspaceResource
{
    public long WorkspaceId { get; set; }
    public DateRange DateRange { get; set; }
}