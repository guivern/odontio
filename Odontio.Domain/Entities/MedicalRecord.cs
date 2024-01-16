using Odontio.Domain.Common;

namespace Odontio.Domain.Entities;

public class MedicalRecord: BaseAuditableEntity
{
    public long Id { get; set; }
    public DateTimeOffset Date { get; set; }
    
    public long PatientId {get; set;}
    public Patient Patient {get; set;}
    
    public long TreatmentId {get; set;}
    public Treatment Treatment {get; set;}
    
    public long? ToothId {get; set;}
    public Tooth Tooth {get; set;}
    
    public decimal Cost {get; set;}
    public bool IsDeleted { get; set; }
    
    public List<Payment> Payments {get; set;}
    public List<Appointment> Appointments {get; set;}
}