using Odontio.Application.Common.Attributes;
using Odontio.Application.Common.Interfaces;
using Odontio.Application.MedicalRecords.Common;

namespace Odontio.Application.MedicalRecords.Commands.CreateMedicalRecord;

[ValidateWorkspace]
[ValidatePatient]
[RolesAuthorize(nameof(RolesEnum.Administrator), nameof(RolesEnum.User))]
public class CreateMedicalRecordCommand : IRequest<ErrorOr<UpsertMedicalRecordResult>>, IPatientResource
{
    public long WorkspaceId { get; set; }
    public long PatientId { get; set; }
    public long AppointmentId { get; set; }
    public long PatientTreatmentId { get; set; }
    public string Description { get; set; } = null!;
    public string? Observations { get; set; }
}