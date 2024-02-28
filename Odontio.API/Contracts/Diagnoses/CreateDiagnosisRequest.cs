namespace Odontio.API.Contracts.Diagnoses;

public class CreateDiagnosisRequest
{
    public DateOnly? Date { get; set; }
    public long? ToothId { get; set; }
    public string Description { get; set; } = null!;
    public string? Observations { get; set; }
}