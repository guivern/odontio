using System.Text;
using JetBrains.Annotations;
using MapsterMapper;
using Moq;
using Moq.EntityFrameworkCore;
using Odontio.Application.Authentication.Common;
using Odontio.Application.Authentication.Queries.Login;
using Odontio.Application.Common.Interfaces;
using Odontio.Domain.Entities;

namespace Odontio.Tests.Authentication.Queries.Login;

[TestSubject(typeof(LoginHandler))]
public class LoginHandlerTest
{
    private readonly Mock<IApplicationDbContext> _mockContext = new();
    private readonly Mock<IAuthService> _mockAuthService = new();
    private readonly Mock<IMapper> _mockMapper = new();

    [Fact]
    public async Task Handle_ValidCredentials_ReturnsAuthenticationResult()
    {
        var user = new User 
        { 
            Username = "test", 
            PasswordHash = Encoding.UTF8.GetBytes("hash"), 
            PasswordSalt = Encoding.UTF8.GetBytes("salt") 
        };
        var token = "token";
        var authResult = new AuthenticationResult(1, "test", "test@email.com", 1, 1, token);

        var users = new List<User> { user };

        _mockContext.Setup(x => x.Users).ReturnsDbSet(users);

        _mockAuthService.Setup(a => a.VerifyPassword(It.IsAny<string>(), user.PasswordHash, user.PasswordSalt))
            .Returns(true);
        _mockAuthService.Setup(a => a.GenerateJwtToken(user))
            .Returns(token);
        _mockMapper.Setup(m => m.Map<AuthenticationResult>(user))
            .Returns(authResult);

        var handler = new LoginHandler(_mockContext.Object, _mockAuthService.Object, _mockMapper.Object);
        var request = new LoginQuery("test", "password");

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(authResult, result);
    }
    
    [Fact]
    public async Task Handle_InvalidCredentials_ReturnsError()
    {
        var user = new User 
        { 
            Username = "test", 
            PasswordHash = Encoding.UTF8.GetBytes("hash"), 
            PasswordSalt = Encoding.UTF8.GetBytes("salt") 
        };

        var users = new List<User> { user };

        _mockContext.Setup(x => x.Users).ReturnsDbSet(users);

        _mockAuthService.Setup(a => a.VerifyPassword(It.IsAny<string>(), user.PasswordHash, user.PasswordSalt))
            .Returns(false); // Invalid password

        var handler = new LoginHandler(_mockContext.Object, _mockAuthService.Object, _mockMapper.Object);
        var request = new LoginQuery("test", "wrongpassword"); // Invalid password

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result.IsError);
        Assert.Equal("INVALID_CREDENTIALS", result.FirstError.Code);
        Assert.Equal("Invalid username or password", result.FirstError.Description);
    }
}