namespace Odontio.Application.Authentication.Common;

public record AuthenticationResult(
    long Id,
    string Username,
    string FirstName,
    string LastName,
    string? Email,
    long? WorkspaceId,
    long? RoleId,
    string RoleName,
    string Token
);