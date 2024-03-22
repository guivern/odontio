using Odontio.Application.Common.Attributes;
using Odontio.Application.Common.Helpers;
using Odontio.Application.Common.Interfaces;
using Odontio.Application.MedicalRecords.Common;

namespace Odontio.Application.MedicalRecords.Queries.GetMedicalRecords;

[ValidateWorkspace]
[RolesAuthorize(nameof(RolesEnum.Administrator), nameof(RolesEnum.User))]
public class GetMedicalRecordsQuery : PagedListQueryBase, IRequest<ErrorOr<PagedList<GetMedicalRecordFullResult>>>, IWorkspaceResource
{
    public long WorkspaceId { get; set; }
    public long? PatientId { get; set; }
}