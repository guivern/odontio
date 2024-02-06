namespace Odontio.Application.Treatments.Queries.GetTreatments;

public class GetTreatmentsResult
{
    public long Id {get; set;}
    public string Name {get; set;} = null!;
    public string? Description {get; set;}
    public decimal? Cost {get; set;}
    public string CategoryName {get; set;}
}