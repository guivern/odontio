using Odontio.Domain.Common;

namespace Odontio.Domain.Entities;

public class Diagnosis: BaseAuditableEntity
{
    public long Id { get; set; }
    public DateTimeOffset Date { get; set; }
    
    public long? ToothId { get; set; }
    public Tooth Tooth { get; set; }
    
    public long PatientId { get; set; }
    public Patient Patient { get; set; }
    
    public string Description { get; set; }
    public string Observations { get; set; }
    
    public bool IsDeleted { get; set; }
}