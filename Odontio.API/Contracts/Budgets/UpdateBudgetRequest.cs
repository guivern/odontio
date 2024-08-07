namespace Odontio.API.Contracts.Budgets;

public class UpdateBudgetRequest
{
    public DateOnly Date { get; set; }
    public DateOnly? ExpirationDate { get; set; }
    public string? Observations { get; set; }
    
    public List<UpdatePatientTreatmentRequest> Details { get; set; } = new();
}

public class UpdatePatientTreatmentRequest
{
    public long? Id { get; set; }
    public BudgetTreatmentRequest Treatment { get; set; } = null!;
    public BudgetDiagnosisRequest? Diagnosis { get; set; }
    public string? Observations { get; set; }
    public decimal Cost { get; set; }
}