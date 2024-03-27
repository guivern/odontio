using Odontio.Domain.Common;

namespace Odontio.Domain.Entities;

public class MedicalNote: BaseAuditableEntity
{
    public long Id { get; set; }
    
    public long PatientTreatmentId { get; set; }
    public PatientTreatment PatientTreatment { get; set; } = null!;
    
    public long AppointmentId { get; set; }
    public Appointment Appointment { get; set; } = null!;
    
    public string? Description {get; set;}
    public string? Observations {get; set;}
}