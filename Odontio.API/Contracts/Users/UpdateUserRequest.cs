namespace Odontio.API.Contracts.Users;

public class UpdateUserRequest
{
    public string? Username { get; set; } = null!;
    public string? Password { get; set; } = null!;
    public string? ConfirmPassword { get; set; } = null!;
    public string? Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? PhotoUrl { get; set; }
    public long RoleId { get; set; }
    public long? WorkspaceId { get; set; }
    public bool IsDoctor { get; set; }
    
}