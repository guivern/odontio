using Odontio.Domain.Common;
using Odontio.Domain.Enums;

namespace Odontio.Domain.Entities;

public class PatientTreatment: BaseAuditableEntity
{
    public long Id { get; set; }
    
    public long BudgetId { get; set; }
    public Budget Budget { get; set; } = null!;
    
    public long TreatmentId { get; set; }
    public Treatment Treatment { get; set; } = null!;
    
    public long? ToothId { get; set; }
    public Tooth? Tooth { get; set; }
    
    public decimal Cost { get; set; }
    
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}