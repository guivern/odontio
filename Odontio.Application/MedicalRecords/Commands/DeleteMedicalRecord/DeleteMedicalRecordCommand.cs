using Odontio.Application.Common.Attributes;
using Odontio.Application.Common.Interfaces;

namespace Odontio.Application.MedicalRecords.Commands.DeleteMedicalRecord;

[ValidateWorkspace]
[ValidatePatient]
[RolesAuthorize(nameof(RolesEnum.Administrator), nameof(RolesEnum.User))]
public class DeleteMedicalRecordCommand : IRequest<ErrorOr<Unit>>, IPatientResource
{
    public long Id { get; set; }
    public long AppointmentId { get; set; }
    public long WorkspaceId { get; set; }
    public long PatientId { get; set; }
}