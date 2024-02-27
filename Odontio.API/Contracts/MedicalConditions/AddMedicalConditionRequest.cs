namespace Odontio.API.Contracts.MedicalConditions;

public class AddMedicalConditionRequest
{
    public string ConditionType { get; set; } = null!;
    public bool HasCondition { get; set; }
    public string? Description { get; set; }
}