namespace Odontio.API.Contracts.Budgets;

public class CreateBudgetRequest
{
    public DateOnly? Date { get; set; }
    public DateOnly? ExpirationDate { get; set; }
    public string? Observations { get; set; }
    public List<CreatePatientTreatmentRequest> Details { get; set; } = new();
}

public class CreatePatientTreatmentRequest
{
    public BudgetTreatmentRequest Treatment { get; set; } = null!;
    public BudgetDiagnosisRequest? Diagnosis { get; set; }
    public string? Observations { get; set; }
    public decimal Cost { get; set; }
}