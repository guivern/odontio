using JetBrains.Annotations;
using Microsoft.Extensions.Options;
using Moq;
using Odontio.Domain.Entities;
using Odontio.Infrastructure.Models;
using Odontio.Infrastructure.Services;

namespace Odontio.Tests.Services;

[TestSubject(typeof(AuthService))]
public class AuthServiceTest
{
    private readonly AuthService _authService;
    private readonly Mock<IOptions<JwtSettings>> _jwtSettingsMock;

    public AuthServiceTest()
    {
        _jwtSettingsMock = new Mock<IOptions<JwtSettings>>();
        _jwtSettingsMock.Setup(x => x.Value).Returns(new JwtSettings
        {
            Secret = "this is my custom Secret key for authnetication", 
            Issuer = "YourIssuer", 
            Audience = "YourAudience", 
            ExpiryMinutes = 60
        });

        _authService = new AuthService(_jwtSettingsMock.Object);
    }

    [Fact]
    public void GenerateJwtToken_ShouldReturnToken_WhenUserIsValid()
    {
        // Arrange
        var user = new User { Id = 1, Username = "TestUser", Email = "test@test.com" };

        // Act
        var token = _authService.GenerateJwtToken(user);

        // Assert
        Assert.NotNull(token);
        Assert.IsType<string>(token);
        
    }

    [Fact]
    public void GeneratePasswordHash_ShouldReturnHash_WhenPasswordIsValid()
    {
        // Arrange
        var password = "TestPassword";
        var salt = _authService.GeneratePasswordSalt();

        // Act
        var hash = _authService.GeneratePasswordHash(password, salt);

        // Assert
        Assert.NotNull(hash);
        Assert.IsType<byte[]>(hash);
    }

    [Fact]
    public void GeneratePasswordSalt_ShouldReturnSalt()
    {
        // Act
        var salt = _authService.GeneratePasswordSalt();

        // Assert
        Assert.NotNull(salt);
        Assert.IsType<byte[]>(salt);
    }

    [Fact]
    public void VerifyPassword_ShouldReturnTrue_WhenPasswordIsValid()
    {
        // Arrange
        var password = "TestPassword";
        var salt = _authService.GeneratePasswordSalt();
        var hash = _authService.GeneratePasswordHash(password, salt);

        // Act
        var result = _authService.VerifyPassword(password, hash, salt);

        // Assert
        Assert.True(result);
    }
    
    [Fact]
    public void GeneratePasswordHash_ShouldReturnDifferentHashes_ForDifferentPasswords()
    {
        // Arrange
        var password1 = "TestPassword1";
        var password2 = "TestPassword2";
        var salt = _authService.GeneratePasswordSalt();

        // Act
        var hash1 = _authService.GeneratePasswordHash(password1, salt);
        var hash2 = _authService.GeneratePasswordHash(password2, salt);

        // Assert
        Assert.NotEqual(hash1, hash2);
    }

    [Fact]
    public void GeneratePasswordHash_ShouldReturnSameHash_ForSamePasswordAndSalt()
    {
        // Arrange
        var password = "TestPassword";
        var salt = _authService.GeneratePasswordSalt();

        // Act
        var hash1 = _authService.GeneratePasswordHash(password, salt);
        var hash2 = _authService.GeneratePasswordHash(password, salt);

        // Assert
        Assert.Equal(hash1, hash2);
    }

    [Fact]
    public void GeneratePasswordSalt_ShouldReturnDifferentSalts_EachTimeItIsCalled()
    {
        // Act
        var salt1 = _authService.GeneratePasswordSalt();
        var salt2 = _authService.GeneratePasswordSalt();

        // Assert
        Assert.NotEqual(salt1, salt2);
    }
}