using Odontio.Application.Common.Attributes;
using Odontio.Application.Common.Interfaces;
using Odontio.Application.Diagnoses.Common;
using Odontio.Domain.Enums;

namespace Odontio.Application.Diagnoses.Queries.GetDiagnoses;

[ValidateWorkspace]
[ValidatePatient]
[RolesAuthorize(nameof(Roles.Administrator))]
public class GetDiagnosesQuery: IRequest<ErrorOr<List<UpsertDiagnosisResult>>>, IPatientResource
{
    public long WorkspaceId { get; set; }
    public long PatientId { get; set; }
}