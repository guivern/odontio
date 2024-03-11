using Odontio.Application.Common.Attributes;
using Odontio.Application.Common.Helpers;
using Odontio.Application.Common.Interfaces;
using Odontio.Application.PatientTreatments.Common;
using Odontio.Domain.Enums;

namespace Odontio.Application.PatientTreatments.Queries.GetPatientTreatmentsByPatient;

[ValidateWorkspace]
[ValidatePatient]
[RolesAuthorize(nameof(RolesEnum.Administrator))]
public class GetPatientTreatmentsByPatientQuery: PagedListQueryBase, IRequest<PagedList<GetPatientTreatmentFullResult>>, IPatientResource
{
    public long PatientId { get; set; }
    public long WorkspaceId { get; set; }
    public TreatmentStatus? Status { get; set; }
}