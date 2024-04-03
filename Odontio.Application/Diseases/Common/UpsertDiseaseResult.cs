namespace Odontio.Application.Diseases.Common;

public class UpsertDiseaseResult
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public long WorkspaceId { get; set; }
}