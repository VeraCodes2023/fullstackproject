using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ECommerceTest;
public class AuthTest
{
    [Fact]
    public void Login_ValidCredential_ReturnsToken()
    {
        var credential = new Credential { Email = "test@example.com", Password = "password123" };

        var mockUserRepo = new Mock<IUserRepo>();
        var mockTokenService = new Mock<ITokenService>();
        var authService = new AuthService(mockUserRepo.Object, mockTokenService.Object);

        var user = new User { Email = "test@example.com", Password = "hashed_password" };
        var generatedToken = "generated_token";

        mockUserRepo.Setup(repo => repo.getByEmail(credential.Email)).Returns(user);
        mockTokenService.Setup(service => service.GenerateToken(user)).Returns(generatedToken); 
        var result = authService.Login(credential);
        Assert.Equal(generatedToken, result);
    }

    [Fact]
    public void Login_InvalidCredential_ThrowsException()
    {

        var credential = new Credential { Email = "nonexistent@example.com", Password = "invalid_password" };
        var mockUserRepo = new Mock<IUserRepo>();
        var mockTokenService = new Mock<ITokenService>();
        var authService = new AuthService(mockUserRepo.Object, mockTokenService.Object);
        mockUserRepo.Setup(repo => repo.getByEmail(credential.Email)).Returns(() => null);
        Assert.Throws<Exception>(() => authService.Login(credential));
    }

    [Fact]
    public void Login_ValidCredentialInvalidPassword_ThrowsException()
    {
        var credential = new Credential { Email = "test@example.com", Password = "invalid_password" };

        var mockUserRepo = new Mock<IUserRepo>();
        var mockTokenService = new Mock<ITokenService>();
        var authService = new AuthService(mockUserRepo.Object, mockTokenService.Object);

        var user = new User { Email = "test@example.com", Password = "hashed_password" };

        mockUserRepo.Setup(repo => repo.getByEmail(credential.Email)).Returns(user);
        Assert.Throws<Exception>(() => authService.Login(credential));
    }
}
