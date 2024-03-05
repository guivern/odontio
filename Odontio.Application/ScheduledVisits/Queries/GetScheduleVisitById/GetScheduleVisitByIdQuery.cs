using Odontio.Application.Common.Attributes;
using Odontio.Application.Common.Interfaces;
using Odontio.Application.ScheduledVisits.Common;
using Odontio.Domain.Enums;

namespace Odontio.Application.ScheduledVisits.Queries.GetScheduleVisitById;

[ValidateWorkspace]
[ValidatePatient]
[RolesAuthorize(nameof(Roles.Administrator))]
public class GetScheduleVisitByIdQuery: IRequest<ErrorOr<UpsertScheduledVisitResult>>, IPatientResource
{
    public long Id { get; set; }
    public long PatientId { get; set; }
    public long WorkspaceId { get; set; }
}