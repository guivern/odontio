namespace Odontio.Application.MedicalConditions.Common;

public class MedicalConditionResult
{
    public long Id { get; set; }
    public string ConditionType { get; set; } = null!;
    public bool HasCondition { get; set; }
    public string? Description { get; set; }
}