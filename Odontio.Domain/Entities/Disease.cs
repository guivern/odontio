namespace Odontio.Domain.Entities;

public class Disease
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public long WorkspaceId { get; set; }
    public Workspace Workspace { get; set; }
    
    public ICollection<PatientDisease> PatientDiseases { get; set; } = new List<PatientDisease>();
}