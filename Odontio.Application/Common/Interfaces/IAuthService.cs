using Odontio.Domain.Entities;

namespace Odontio.Application.Common.Interfaces;

public interface IAuthService
{
    string GenerateJwtToken(User user);
    byte[] GeneratePasswordSalt();
    byte[] GeneratePasswordHash(string password, byte[] salt);
    bool VerifyPassword(string password, byte[] passwordHash, byte[] passwordSalt);
    long GetCurrentUserId();
    string GetCurrentUserName();
}