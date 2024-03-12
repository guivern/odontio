using Odontio.Application.Common.Attributes;
using Odontio.Application.Common.Helpers;
using Odontio.Application.Common.Interfaces;
using Odontio.Application.PatientTreatments.Common;
using Odontio.Domain.Enums;

namespace Odontio.Application.PatientTreatments.Queries.GetPatientTreatments;

[ValidateWorkspace]
[RolesAuthorize(nameof(RolesEnum.Administrator))]
public class GetPatientTreatmentsQuery: PagedListQueryBase, IRequest<ErrorOr<PagedList<GetPatientTreatmentFullResult>>>, IWorkspaceResource
{
    public long WorkspaceId { get; set; }
    public long? PatientId { get; set; }
    public TreatmentStatus? Status { get; set; }
}