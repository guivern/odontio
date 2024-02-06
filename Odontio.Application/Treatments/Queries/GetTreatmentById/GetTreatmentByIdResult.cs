namespace Odontio.Application.Treatments.Queries.GetTreatmentById;

public class GetTreatmentByIdResult
{
    public long Id {get; set;}
    public string Name {get; set;} = null!;
    public string? Description {get; set;}
    public decimal? Cost {get; set;}
    public long WorkspaceId {get; set;}
    public long CategoryId {get; set;}
}