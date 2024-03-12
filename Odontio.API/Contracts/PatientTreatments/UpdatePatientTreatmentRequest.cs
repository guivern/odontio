namespace Odontio.API.Contracts.PatientTreatments;

public class UpdatePatientTreatmentRequest
{
    public long Id { get; set; }
    public long TreatmentId { get; set; }
    public long? ToothId { get; set; }
    public decimal Cost { get; set; }
    public string Status { get; set; }
}