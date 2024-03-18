namespace Odontio.Application.PatientTreatments.Common;

public class GetPatientTreatmentResult
{
    public long Id { get; set; }
    public long TreatmentId { get; set; }
    public string TreatmentName { get; set; }
    public long? ToothId { get; set; }
    public string? ToothName { get; set; }
    public decimal Cost { get; set; }
    public long BudgetId { get; set; }
    public string Status { get; set; }
    public long PatientId { get; set; }
    public string PatientName { get; set; }
}