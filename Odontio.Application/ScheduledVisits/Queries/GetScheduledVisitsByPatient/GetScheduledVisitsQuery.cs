using Odontio.Application.Common.Attributes;
using Odontio.Application.Common.Interfaces;
using Odontio.Application.ScheduledVisits.Common;

namespace Odontio.Application.ScheduledVisits.Queries.GetScheduledVisitsByPatient;

[ValidateWorkspace]
[ValidatePatient]
[RolesAuthorize(nameof(RolesEnum.Administrator))]
public class GetScheduledVisitsQuery: IRequest<IEnumerable<ScheduledVisitDto>>, IPatientResource
{
    public long WorkspaceId { get; set; }
    public long PatientId { get; set; }
}