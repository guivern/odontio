namespace Odontio.Domain.Entities;

public class User
{
    public long Id { get; set; }
    public string Username { get; set; } = null!;
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhotoUrl { get; set; }
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }
    public bool IsDeleted { get; set; }
    
    public long WorkspaceId { get; set; }
    public Workspace Workspace { get; set; }
    
    public long RoleId { get; set; }
    public Role Role { get; set; }
}