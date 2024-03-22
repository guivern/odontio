namespace Odontio.Application.Users.Queries.GetUserById;

public class GetUserByIdResult
{
    public long Id { get; set; }
    public string Username { get; set; } = null!;
    public string? Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? PhotoUrl { get; set; }
    public long WorkspaceId { get; set; }
    public string WorkspaceName { get; set; }
    public long RoleId { get; set; }
    public bool IsActive { get; set; }
    public string RoleName { get; set; }
}