using Odontio.Application.Appointments.Common;
using Odontio.Application.Common.Attributes;
using Odontio.Application.Common.Interfaces;

namespace Odontio.Application.Appointments.Commands.CreateAppointment;

[ValidateWorkspace]
[ValidatePatient]
[RolesAuthorize(nameof(RolesEnum.Administrator))]
public class CreateAppointmentCommand: IRequest<ErrorOr<UpsertAppointmentResult>>, IPatientResource
{
    public long WorkspaceId { get; set; }
    public long PatientId { get; set; }
    public DateTimeOffset? Date {get; set;}
    
    public ICollection<CreateMedicalRecordCommand> MedicalRecords { get; set; } = new List<CreateMedicalRecordCommand>();
}

public class CreateMedicalRecordCommand
{
    public long PatientTreatmentId { get; set; }
    public string? Description {get; set;}
    public string? Observations {get; set;}
}