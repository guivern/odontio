using Odontio.Domain.Common;

namespace Odontio.Domain.Entities;

public class Appointment: BaseAuditableEntity
{
    public long Id { get; set; }
    public DateTimeOffset Date {get; set;}
    
    public long PatientTreatmentId { get; set; }
    public PatientTreatment PatientTreatment { get; set; } = null!;
    
    public string? Description {get; set;}
    public string? Observations {get; set;}
}