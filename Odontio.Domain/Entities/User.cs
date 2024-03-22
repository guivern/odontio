using Odontio.Domain.Common;

namespace Odontio.Domain.Entities;

public class User: BaseAuditableEntity
{
    public long Id { get; set; }
    public string Username { get; set; } = null!;
    public string? Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? PhotoUrl { get; set; }
    public byte[] PasswordHash { get; set; } = null!;
    public byte[] PasswordSalt { get; set; } = null!;
    
    public long? WorkspaceId { get; set; }
    public Workspace Workspace { get; set; } = null!;
    
    public long RoleId { get; set; }
    public Role Role { get; set; } = null!;
    public bool IsActive { get; set; } = true;
    public bool IsDoctor { get; set; }
}