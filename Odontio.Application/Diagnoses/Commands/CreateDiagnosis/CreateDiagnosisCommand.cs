using Odontio.Application.Common.Attributes;
using Odontio.Application.Common.Interfaces;
using Odontio.Application.Diagnoses.Common;

namespace Odontio.Application.Diagnoses.Commands.CreateDiagnosis;

[ValidateWorkspace]
[ValidatePatient]
[RolesAuthorize(nameof(RolesEnum.Administrator))]
public class CreateDiagnosisCommand: IRequest<ErrorOr<UpsertDiagnosisResult>>, IPatientResource
{
    public DateOnly? Date { get; set; }
    public long? ToothId { get; set; }
    public string Description { get; set; } = null!;
    public string? Observations { get; set; }
    public long WorkspaceId { get; set; }
    public long PatientId { get; set; }
}