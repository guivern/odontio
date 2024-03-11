using Odontio.Application.Common.Attributes;
using Odontio.Application.Common.Helpers;
using Odontio.Application.Common.Interfaces;
using Odontio.Application.PatientTreatments.Common;
using Odontio.Domain.Enums;

namespace Odontio.Application.PatientTreatments.Queries.GetPatientTreatmentsByWorkspace;

[ValidateWorkspace]
[RolesAuthorize(nameof(RolesEnum.Administrator))]
public class GetPatientTreatmentsByWorkspaceQuery: PagedListQueryBase, IRequest<PagedList<GetPatientTreatmentFullResult>>, IWorkspaceResource
{
    public long WorkspaceId { get; set; }
    public TreatmentStatus? Status { get; set; }
}