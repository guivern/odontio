using Odontio.Application.Appointments.Common;
using Odontio.Application.Common.Attributes;
using Odontio.Application.Common.Interfaces;
using Odontio.Application.MedicalRecords.Common;

namespace Odontio.Application.MedicalRecords.Queries.GetMedicalRecordById;

[ValidateWorkspace]
[ValidatePatient]
[RolesAuthorize(nameof(RolesEnum.Administrator), nameof(RolesEnum.User))]
public class GetMedicalRecordByIdQuery : IRequest<ErrorOr<GetMedicalRecordFullResult>>, IPatientResource
{
    public long Id { get; set; }
    public long WorkspaceId { get; set; }
    public long PatientId { get; set; }
}