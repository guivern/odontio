using Odontio.Domain.Common;

namespace Odontio.Domain.Entities;

public class ScheduledAppointment: BaseAuditableEntity
{
    public long Id {get; set;}
    public DateTime Date {get; set;}
    public string? Description {get; set;}
    
    public long PatientId {get;set;}
    public Patient Patient {get;set;} = null!;
}