namespace Odontio.API.Contracts.Diagnoses;

public class UpdateDiagnosisRequest
{
    public long Id { get; set; }
    public DateOnly? Date { get; set; }
    public long? ToothId { get; set; }
    public string Description { get; set; } = null!;
    public string? Observations { get; set; }
}