namespace Odontio.Application.Profile.Queries.GetProfile;

public class GetProfileResult
{
    public string Username { get; set; } = null!;
    public string? Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? PhotoUrl { get; set; }
    public string WorkspaceName { get; set; }
    public string WorkspaceId { get; set; }
    public string RoleId { get; set; }
    public string RoleName { get; set; }
    public bool IsDoctor { get; set; }
}