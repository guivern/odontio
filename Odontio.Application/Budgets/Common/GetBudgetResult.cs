namespace Odontio.Application.Budgets.Common;

public class GetBudgetResult
{
    public long Id { get; set; }
    public DateOnly Date { get; set; }
    public string Status { get; set; } = null!;
    public DateOnly ExpirationDate { get; set; }
    public long PatientId { get; set; }
    public string PatientName { get; set; }
    public decimal TotalCost { get; set; }

    public List<GetPatientTreatmentResult> PatientTreatments { get; set; } = new();
}

public class GetPatientTreatmentResult
{
    public long Id { get; set; }
    public long TreatmentId { get; set; }
    public string TreatmentName { get; set; }
    public long? ToothId { get; set; }
    public decimal Cost { get; set; }
}