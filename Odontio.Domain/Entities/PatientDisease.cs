namespace Odontio.Domain.Entities;

public class PatientDisease
{
    public long Id { get; set; }

    public long PatientId { get; set; }
    public Patient Patient { get; set; } = null!;

    public long DiseaseId { get; set; }
    public Disease Disease { get; set; } = null!;
}