namespace Odontio.API.Contracts.MedicalConditions;

public class UpdateMedicalConditionRequest
{
    public string ConditionType { get; set; } = null!;
    public bool? HasCondition { get; set; }
    public string? Description { get; set; }
}