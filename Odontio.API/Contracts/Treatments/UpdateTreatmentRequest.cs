namespace Odontio.API.Contracts.Treatments;

public class UpdateTreatmentRequest
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public decimal? Cost { get; set; }
    public long CategoryId { get; set; }
    public long WorkspaceId { get; set; }
}