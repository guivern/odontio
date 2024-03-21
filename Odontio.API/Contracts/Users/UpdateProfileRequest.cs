namespace Odontio.API.Contracts.Users;

public class UpdateProfileRequest
{
    public string Username { get; set; } = null!;
    public string? Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? PhotoUrl { get; set; }
}