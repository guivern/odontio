namespace Odontio.Domain.Entities;

public class Treatment
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public decimal? Cost { get; set; }

    public long WorkspaceId { get; set; }
    public Workspace Workspace { get; set; } = null!;

    public long CategoryId { get; set; }
    public Category Category { get; set; } = null!;

    public ICollection<PatientTreatment> PatientTreatments { get; set; } = new List<PatientTreatment>();
}