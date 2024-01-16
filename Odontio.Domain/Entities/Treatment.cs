namespace Odontio.Domain.Entities;

public class Treatment
{
    public long Id {get; set;}
    public string Name {get; set;}
    public string Description {get; set;}
    public decimal? Cost {get; set;}
    
    public long WorkspaceId {get; set;}
    public Workspace Workspace {get; set;}
}