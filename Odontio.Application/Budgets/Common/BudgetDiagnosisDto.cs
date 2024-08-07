using Odontio.Domain.Enums;

namespace Odontio.Application.Budgets.Common;

public class BudgetDiagnosisDto
{
    public long? Id { get; set; }
    public DateOnly? Date { get; set; }
    public long? ToothId { get; set; }
    public string? ToothName { get; set; }
    public OdontogramType OdontogramType {get; set;}
    public string Description { get; set; } = null!;
    public string? Observations { get; set; }
}