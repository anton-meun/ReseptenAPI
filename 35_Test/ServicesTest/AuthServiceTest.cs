using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Xunit;
using _20_Business.Services;
using _30_Data.Entities;

public class AuthServiceTests
{
    private IConfiguration GetTestConfiguration()
    {
        // Helper method to create a test configuration with a mock secret key and issuer
        var inMemorySettings = new Dictionary<string, string>
    {
        { "Jwt:Key", "SuperSecretTestKeyThatIsAtLeast32Chars!" },
        { "Jwt:Issuer", "TestIssuer" }
    };

        return new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();
    }

    [Fact]
    public void GenerateJwtToken_Should_Return_Valid_Token()
    {
        // Arrange
        var config = GetTestConfiguration();
        var authService = new AuthService(config);
        var testUser = new User { Id = 1, Email = "test@example.com" };

        // Act: Generate a JWT token
        var token = authService.GenerateJwtToken(testUser);

        // Assert
        Assert.NotNull(token);
        Assert.False(string.IsNullOrEmpty(token));
    }

    [Fact]
    public void GenerateJwtToken_Should_Contain_Correct_Claims()
    {
        // Arrange
        var config = GetTestConfiguration();
        var authService = new AuthService(config);
        var testUser = new User { Id = 1, Email = "test@example.com" };

        // Act: Generate a JWT token and parse it
        var token = authService.GenerateJwtToken(testUser);
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.ReadJwtToken(token);

        // Assert
        Assert.Equal("TestIssuer", jwtToken.Issuer);
        Assert.Contains(jwtToken.Claims, c => c.Type == JwtRegisteredClaimNames.Sub && c.Value == "1");
        Assert.Contains(jwtToken.Claims, c => c.Type == JwtRegisteredClaimNames.Email && c.Value == "test@example.com");
    }

    [Fact]
    public void GenerateJwtToken_Should_Have_Valid_Expiration()
    {
        // Arrange
        var config = GetTestConfiguration();
        var authService = new AuthService(config);
        var testUser = new User { Id = 1, Email = "test@example.com" };

        // Act
        var token = authService.GenerateJwtToken(testUser);
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.ReadJwtToken(token);

        // Assert: Ensure the token expiration time is in the future
        Assert.True(jwtToken.ValidTo > DateTime.UtcNow);
    }
}
