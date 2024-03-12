namespace Odontio.API.Contracts.Budgets;

public class UpdateBudgetRequest
{
    public DateOnly Date { get; set; }
    
    public List<UpdatePatientTreatmentRequest> PatientTreatments { get; set; } = new();
}

public class UpdatePatientTreatmentRequest
{
    public long Id { get; set; }
    public long TreatmentId { get; set; }
    public long? ToothId { get; set; }
    public decimal Cost { get; set; }
    public string Status { get; set; }
}