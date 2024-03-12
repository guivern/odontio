namespace Odontio.API.Contracts.PatientTreatments;

public class CreatePatientTreatmentRequest
{
    public long TreatmentId { get; set; }
    public long? ToothId { get; set; }
    public decimal Cost { get; set; }
}