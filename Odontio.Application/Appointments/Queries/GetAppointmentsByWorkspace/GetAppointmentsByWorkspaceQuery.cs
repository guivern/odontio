using Odontio.Application.Appointments.Common;
using Odontio.Application.Common.Attributes;
using Odontio.Application.Common.Helpers;
using Odontio.Application.Common.Interfaces;

namespace Odontio.Application.Appointments.Queries.GetAppointmentsByWorkspace;

[ValidateWorkspace]
[RolesAuthorize(nameof(RolesEnum.Administrator))]
public class GetAppointmentsByWorkspaceQuery : PagedListQueryBase, IRequest<PagedList<GetAppointmentResult>>, IWorkspaceResource
{
    public long WorkspaceId { get; set; }
}