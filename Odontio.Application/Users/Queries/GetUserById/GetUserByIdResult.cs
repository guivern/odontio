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
    public long WorkspaceName { get; set; }
    public long RoleId { get; set; }
    public long RoleName { get; set; }
}