using Odontio.Domain.Common;

namespace Odontio.Domain.Entities;

public class Diagnosis: BaseAuditableEntity
{
    public long Id { get; set; }
    public DateOnly Date { get; set; }
    
    public long? ToothId { get; set; }
    public Tooth? Tooth { get; set; }
    
    public long PatientId { get; set; }
    public Patient Patient { get; set; } = null!;
    
    public string Description { get; set; } = null!;
    public string? Observations { get; set; }
    
    public ICollection<PatientTreatment> PatientTreatments { get; set; } = new List<PatientTreatment>();
}