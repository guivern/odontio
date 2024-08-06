namespace Odontio.API.Contracts.Budgets;

public class BudgetDiagnosisRequest
{
    public long? Id { get; set; }
    public DateOnly? Date { get; set; }
    public long? ToothId { get; set; }
    public string Description { get; set; } = null!;
    public string? Observations { get; set; }
}