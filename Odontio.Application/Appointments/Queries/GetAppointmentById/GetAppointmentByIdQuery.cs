using Odontio.Application.Appointments.Common;
using Odontio.Application.Common.Attributes;
using Odontio.Application.Common.Interfaces;

namespace Odontio.Application.Appointments.Queries.GetAppointmentById;

[ValidateWorkspace]
[ValidatePatient]
[RolesAuthorize(nameof(RolesEnum.Administrator))]
public class GetAppointmentByIdQuery : IRequest<ErrorOr<GetAppointmentFullResult>>, IPatientResource
{
    public long Id { get; set; }
    public long WorkspaceId { get; set; }
    public long PatientId { get; set; }
}