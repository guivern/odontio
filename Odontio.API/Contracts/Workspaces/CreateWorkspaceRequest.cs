namespace Odontio.API.Contracts.Workspaces;

public class CreateWorkspaceRequest
{
    public string Name { get; set; } = null!;
    public string? Address { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? Ruc { get; set; }
    public string? ContactName { get; set; }
    public string? ContactPhoneNumber { get; set; }
}