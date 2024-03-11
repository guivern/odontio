namespace Odontio.Application.PatientTreatments.Common;

public class UpsertPatientTreatmentResult
{
    public long Id { get; set; }
    public long TreatmentId { get; set; }
    public long? ToothId { get; set; }
    public decimal Cost { get; set; }
}