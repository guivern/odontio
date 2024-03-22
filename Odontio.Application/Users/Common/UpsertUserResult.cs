namespace Odontio.Application.Users.Common;

public class UpsertUserResult
{
    public long Id { get; set; }
    public string Username { get; set; } = null!;
    public string? Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? PhotoUrl { get; set; }
    public long WorkspaceId { get; set; }
    public long RoleId { get; set; }
    public bool IsActive { get; set; }
}