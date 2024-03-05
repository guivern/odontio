namespace Odontio.API.Contracts.Budgets;

public class CreateBudgetRequest
{
    public DateOnly? Date { get; set; }
    public List<CreatePatientTreatmentRequest> PatientTreatments { get; set; } = new();
}

public class CreatePatientTreatmentRequest
{
    public long TreatmentId { get; set; }
    public long? ToothId { get; set; }
    public decimal Cost { get; set; }
}