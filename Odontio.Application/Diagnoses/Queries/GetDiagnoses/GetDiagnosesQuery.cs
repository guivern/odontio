using Odontio.Application.Common.Attributes;
using Odontio.Application.Common.Helpers;
using Odontio.Application.Common.Interfaces;
using Odontio.Application.Diagnoses.Common;
using Odontio.Domain.Enums;

namespace Odontio.Application.Diagnoses.Queries.GetDiagnoses;

[ValidateWorkspace]
[ValidatePatient]
[RolesAuthorize(nameof(RolesEnum.Administrator), nameof(RolesEnum.User))]
public class GetDiagnosesQuery: PagedListQueryBase, IRequest<ErrorOr<PagedList<UpsertDiagnosisResult>>>, IPatientResource
{
    public long WorkspaceId { get; set; }
    public long PatientId { get; set; }
    public long? ToothId { get; set; }
}