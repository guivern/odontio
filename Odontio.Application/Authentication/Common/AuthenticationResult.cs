namespace Odontio.Application.Authentication.Common;

public record AuthenticationResult(
    long Id,
    string Username,
    string? Email,
    long? WorkspaceId,
    long? RoleId,
    string Token
);