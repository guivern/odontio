using Odontio.Application.Common.Attributes;
using Odontio.Application.Common.Interfaces;
using Odontio.Application.MedicalNotes.Common;

namespace Odontio.Application.MedicalNotes.Commands.UpdateMedicalNote;

[ValidateWorkspace]
[ValidatePatient]
[RolesAuthorize(nameof(RolesEnum.Administrator), nameof(RolesEnum.User))]
public class UpdateMedicalNoteCommand : IRequest<ErrorOr<UpsertMedicalNoteResult>>, IPatientResource
{
    public long Id { get; set; }
    public long WorkspaceId { get; set; }
    public long PatientId { get; set; }
    public long AppointmentId { get; set; }
    public long PatientTreatmentId { get; set; }
    public string Description { get; set; } = null!;
    public string? Observations { get; set; }
}