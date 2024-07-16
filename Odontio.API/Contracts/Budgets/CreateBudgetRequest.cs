namespace Odontio.API.Contracts.Budgets;

public class CreateBudgetRequest
{
    public DateOnly? Date { get; set; }
    public DateOnly ExpirationDate { get; set; }
    public string? Observations { get; set; }
    public List<CreatePatientTreatmentRequest> PatientTreatments { get; set; } = new();
}

public class CreatePatientTreatmentRequest
{
    public long TreatmentId { get; set; }
    public long? DiagnosisId { get; set; }
    public decimal Cost { get; set; }
}