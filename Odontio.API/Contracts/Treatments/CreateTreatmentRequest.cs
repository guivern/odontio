namespace Odontio.API.Contracts.Treatments;

public class CreateTreatmentRequest
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public decimal? Cost { get; set; }
    public long CategoryId { get; set; }
}