namespace Odontio.Domain.Entities;

public class MedicalConditionQuestion
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public long WorkspaceId { get; set; }
    public Workspace Workspace { get; set; } = null!;
}