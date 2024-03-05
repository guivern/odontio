using Odontio.Application.Common.Attributes;
using Odontio.Application.Common.Interfaces;
using Odontio.Domain.Enums;

namespace Odontio.Application.Diagnoses.Commands.DeleteDiagnosis;

[ValidateWorkspace]
[ValidatePatient]
[RolesAuthorize(nameof(Roles.Administrator))]
public class DeleteDiagnosisCommand: IRequest<ErrorOr<Unit>>, IPatientResource
{
    public long WorkspaceId { get; set; }
    public long PatientId { get; set; }
    public long Id { get; set; }
}