using Odontio.Application.Appointments.Common;
using Odontio.Application.Common.Attributes;
using Odontio.Application.Common.Helpers;
using Odontio.Application.Common.Interfaces;

namespace Odontio.Application.Appointments.Queries.GetAppointments;

[ValidateWorkspace]
[RolesAuthorize(nameof(RolesEnum.Administrator))]
public class GetAppointmentsQuery : PagedListQueryBase, IRequest<ErrorOr<PagedList<GetAppointmentResult>>>, IWorkspaceResource
{
    public long WorkspaceId { get; set; }
    public long? PatientId { get; set; }
}