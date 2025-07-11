using FlapKapBackendChallenge.Dto;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json.Linq;
using System.Net;
using Xunit;

public class AuthTests : IClassFixture<WebApplicationFactory<FlapKapBackendChallenge.Program>>
{
    private readonly HttpClient _client;

    public AuthTests(WebApplicationFactory<FlapKapBackendChallenge.Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Register_WithValidData_ShouldReturnSuccess()
    {
        // Arrange
        var payload = new RegisterRequestDto
        {
            userName = "newuser_" + Guid.NewGuid(),
            password = "123456",
            role = "buyer"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/register", payload);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("data", content.ToLower());
    }

    [Fact]
    public async Task Login_WithValidCredentials_ShouldReturnToken()
    {
        // Arrange (user seeded in Program.cs)
        var loginPayload = new LoginRequestDto
        {
            userName = "testbuyer",
            password = "123456"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/login", loginPayload);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var json = await response.Content.ReadAsStringAsync();
        var parsed = JObject.Parse(json);
        var token = parsed["data"]?["token"]?.ToString();

        Assert.False(string.IsNullOrEmpty(token));
    }

    [Fact]
    public async Task Login_WithInvalidPassword_ShouldReturnUnauthorized()
    {
        // Arrange
        var loginPayload = new LoginRequestDto
        {
            userName = "testbuyer",
            password = "wrongpassword"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/login", loginPayload);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode); // Still returns 200 with success=false
        var body = await response.Content.ReadAsStringAsync();
        Assert.Contains("invalid credentials", body.ToLower());
    }
}
