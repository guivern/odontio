namespace Odontio.Application.MedicalRecords.Common;

public class UpsertMedicalRecordResult
{
    public long Id { get; set; }
    public long AppointmentId { get; set; }
    public long PatientTreatmentId { get; set; }
    public string Description { get; set; } = null!;
    public string? Observations { get; set; }
}