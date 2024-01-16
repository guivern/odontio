namespace Odontio.Application.Authentication.Common;

public record AuthenticationResult(
    long Id,
    string Username,
    string? Email,
    string? PhotoUrl,
    string Token
);