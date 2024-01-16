namespace Odontio.Domain.Entities;

public class MedicalCondition
{
    public long Id { get; set; }
    public string ConditionType { get; set; }
    public bool HasCondition { get; set; }
    public string Description { get; set; }

    public long PatientId { get; set; }
    public Patient Patient { get; set; }
}