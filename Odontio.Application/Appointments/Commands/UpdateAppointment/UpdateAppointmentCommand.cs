using Odontio.Application.Appointments.Commands.CreateAppointment;
using Odontio.Application.Appointments.Common;
using Odontio.Application.Common.Attributes;
using Odontio.Application.Common.Interfaces;

namespace Odontio.Application.Appointments.Commands.UpdateAppointment;

[ValidateWorkspace]
[ValidatePatient]
[RolesAuthorize(nameof(RolesEnum.Administrator))]
public class UpdateAppointmentCommand : IRequest<ErrorOr<UpsertAppointmentResult>>, IPatientResource
{
    public long Id { get; set; }
    public long WorkspaceId { get; set; }
    public long PatientId { get; set; }
    public DateTimeOffset? Date { get; set; }
    
    public ICollection<UpdateMedicalRecordCommand> MedicalRecords { get; set; } = new List<UpdateMedicalRecordCommand>();
}

public class UpdateMedicalRecordCommand : CreateMedicalRecordCommand
{
    public long Id { get; set; }
}