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
    [Fact]
    public async Task Handle_ValidCredentials_ReturnsAuthenticationResult()
    {
        // Arrange
        var mockContext = new Mock<IApplicationDbContext>();
        var mockAuthService = new Mock<IAuthService>();
        var mockMapper = new Mock<IMapper>();

        var user = new User 
        { 
            Username = "test", 
            PasswordHash = Encoding.UTF8.GetBytes("hash"), 
            PasswordSalt = Encoding.UTF8.GetBytes("salt") 
        };
        var token = "token";
        var authResult = new AuthenticationResult(1, "test", "test@email.com", "photoUrl", token);

        var users = new List<User> { user };

        mockContext.Setup(x => x.Users).ReturnsDbSet(users);

        mockAuthService.Setup(a => a.VerifyPassword(It.IsAny<string>(), user.PasswordHash, user.PasswordSalt))
            .Returns(true);
        mockAuthService.Setup(a => a.GenerateJwtToken(user))
            .Returns(token);
        mockMapper.Setup(m => m.Map<AuthenticationResult>(user))
            .Returns(authResult);

        var handler = new LoginHandler(mockContext.Object, mockAuthService.Object, mockMapper.Object);
        var request = new LoginQuery("test", "password");

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(authResult, result);
    }
    
    [Fact]
    public async Task Handle_InvalidCredentials_ReturnsError()
    {
        // Arrange
        var mockContext = new Mock<IApplicationDbContext>();
        var mockAuthService = new Mock<IAuthService>();
        var mockMapper = new Mock<IMapper>();

        var user = new User 
        { 
            Username = "test", 
            PasswordHash = Encoding.UTF8.GetBytes("hash"), 
            PasswordSalt = Encoding.UTF8.GetBytes("salt") 
        };

        var users = new List<User> { user };

        mockContext.Setup(x => x.Users).ReturnsDbSet(users);

        mockAuthService.Setup(a => a.VerifyPassword(It.IsAny<string>(), user.PasswordHash, user.PasswordSalt))
            .Returns(false); // Invalid password

        var handler = new LoginHandler(mockContext.Object, mockAuthService.Object, mockMapper.Object);
        var request = new LoginQuery("test", "wrongpassword"); // Invalid password

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result.IsError);
        Assert.Equal("INVALID_CREDENTIALS", result.FirstError.Code);
        Assert.Equal("Invalid username or password", result.FirstError.Description);
    }
}