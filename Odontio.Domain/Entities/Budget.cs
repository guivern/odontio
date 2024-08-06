using Odontio.Domain.Common;
using Odontio.Domain.Enums;

namespace Odontio.Domain.Entities;

public class Budget: BaseAuditableEntity
{
    // private const int ExpirationMonths = 6;
    public long Id { get; set; }
    public DateOnly Date { get; set; }
    public BudgetStatus Status { get; set; }

    public DateOnly? ExpirationDate { get; set; }
    public string? Observations { get; set; }
    
    public long PatientId { get; set; }
    public Patient Patient { get; set; } = null!;
    
    public ICollection<PatientTreatment> PatientTreatments { get; set; } = new List<PatientTreatment>();
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    
    public void UpdateStatus()
    {
        if (HasAppointment())
        {
            Status = BudgetStatus.Approved;
        }
        else if (HasExpired())
        {
            Status = BudgetStatus.Rejected;
        }
        else
        {
            Status = BudgetStatus.Pending;
        }
    }
    
    public bool CanBeDeleted()
    {
        // Un Budget sólo puede ser eliminado si no tiene citas ni pagos asociados
        return !PatientTreatments.Any(x => x.MedicalNotes.Count > 0) && Payments.Count == 0;
    }
    
    private bool HasAppointment()
    {
        return PatientTreatments.Any(x => x.MedicalNotes.Count > 0);
    }
    
    private bool HasExpired()
    {
        return Status == BudgetStatus.Pending && DateOnly.FromDateTime(DateTimeOffset.Now.Date) > ExpirationDate;
    }
}