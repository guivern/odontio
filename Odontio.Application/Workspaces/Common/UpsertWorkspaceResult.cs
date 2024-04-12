namespace Odontio.Application.Workspaces.Common;

public class UpsertWorkspaceResult
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Address { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? Ruc { get; set; }
    public string? ContactName { get; set; }
    public string? ContactPhoneNumber { get; set; }
    public string? BusinessName { get; set; }
}