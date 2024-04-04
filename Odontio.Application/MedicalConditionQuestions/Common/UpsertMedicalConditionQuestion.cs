namespace Odontio.Application.MedicalConditionQuestions.Common;

public class UpsertMedicalConditionQuestion
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public long WorkspaceId { get; set; }
}