using Odontio.Application.Appointments.Common;
using Odontio.Application.Common.Attributes;
using Odontio.Application.Common.Interfaces;

namespace Odontio.Application.Appointments.Commands.CreateAppointment;

[ValidateWorkspace]
[ValidatePatient]
[RolesAuthorize(nameof(RolesEnum.Administrator), nameof(RolesEnum.User))]
public class CreateAppointmentCommand: IRequest<ErrorOr<UpsertAppointmentResult>>, IPatientResource
{
    public long WorkspaceId { get; set; }
    public long PatientId { get; set; }
    public DateTimeOffset? Date {get; set;}
    
    public ICollection<CreateMedicalRecordDto> MedicalRecords { get; set; } = new List<CreateMedicalRecordDto>();
}

public class CreateMedicalRecordDto
{
    public long PatientTreatmentId { get; set; }
    public string? Description {get; set;}
    public string? Observations {get; set;}
}