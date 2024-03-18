namespace Odontio.Application.Budgets.Common;

public class GetBudgetResultDto
{
    public long Id { get; set; }
    public DateOnly Date { get; set; }
    public string Status { get; set; } = null!;
    public DateOnly ExpirationDate { get; set; }
    public long PatientId { get; set; }
    public string PatientName { get; set; }
    public decimal TotalCost { get; set; }
    public decimal TotalPayments { get; set; }
    public decimal Balance { get; set; }
}