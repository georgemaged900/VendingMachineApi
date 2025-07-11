using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FlapKapBackendChallenge;
using FlapKap.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json.Linq;
using Xunit;

public class RoleTests : IClassFixture<WebApplicationFactory<FlapKapBackendChallenge.Program>>
{
    private readonly HttpClient _client;

    public RoleTests(WebApplicationFactory<FlapKapBackendChallenge.Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetRoles_ShouldReturnListOfRoles()
    {
        // Act
        var response = await _client.GetAsync("/role");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("buyer", content.ToLower());
        Assert.Contains("seller", content.ToLower());
    }

    [Fact]
    public async Task GetRole_WithValidRoleName_ShouldReturnRole()
    {
        // Act
        var response = await _client.GetAsync("/role/buyer");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("buyer", content.ToLower());
    }

    [Fact]
    public async Task AddRoleToUser_ShouldAssignRole()
    {
        // Arrange
        var payload = new UserRole
        {
            userId = 1,        // Make sure this matches seeded user ID
            roleId = 2         // Assign "seller" to testbuyer (for testing)
        };

        // Act
        var response = await _client.PostAsJsonAsync("/role/user", payload);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("data", content.ToLower());
    }
}
