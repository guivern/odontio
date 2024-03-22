using Odontio.Application.Common.Attributes;
using Odontio.Application.Common.Helpers;
using Odontio.Application.Common.Interfaces;
using Odontio.Application.ScheduledVisits.Common;
using Odontio.Domain.Enums;

namespace Odontio.Application.ScheduledVisits.Queries.GetScheduledVisitsByPatient;

[ValidateWorkspace]
[ValidatePatient]
[RolesAuthorize(nameof(RolesEnum.Administrator), nameof(RolesEnum.User))]
public class GetScheduledVisitsQuery: IRequest<ErrorOr<IEnumerable<UpsertScheduledVisitResult>>>, IPatientResource
{
    public long WorkspaceId { get; set; }
    public long PatientId { get; set; }
    public DateRange DateRange { get; set; }
}