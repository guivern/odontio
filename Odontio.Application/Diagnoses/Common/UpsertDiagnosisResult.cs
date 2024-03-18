namespace Odontio.Application.Diagnoses.Common;

public class UpsertDiagnosisResult
{
    public long Id { get; set; }
    public DateOnly Date { get; set; }
    public long? ToothId { get; set; }
    public string ToothName { get; set; } = null!;
    public string ToothType { get; set; } = null!;
    public string Odontogram { get; set; } = null!;
    public long PatientId { get; set; }
    public string Description { get; set; } = null!;
    public string? Observations { get; set; }
}