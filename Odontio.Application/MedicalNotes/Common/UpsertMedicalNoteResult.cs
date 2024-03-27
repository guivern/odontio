namespace Odontio.Application.MedicalNotes.Common;

public class UpsertMedicalNoteResult
{
    public long Id { get; set; }
    public long AppointmentId { get; set; }
    public long PatientTreatmentId { get; set; }
    public string Description { get; set; } = null!;
    public string? Observations { get; set; }
}