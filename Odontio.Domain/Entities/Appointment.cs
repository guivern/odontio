using Odontio.Domain.Common;

namespace Odontio.Domain.Entities;

public class Appointment: BaseAuditableEntity
{
    public long Id {get; set;}
    public DateTimeOffset Date {get; set;}
    public string Description {get; set;}
    public string Observations {get; set;}
    
    public long MedicalRecordId {get;set;}
    public MedicalRecord MedicalRecord {get;set;}
    
    public bool IsDeleted { get; set; }
}