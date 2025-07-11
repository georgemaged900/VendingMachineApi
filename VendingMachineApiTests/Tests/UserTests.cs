using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FlapKapBackendChallenge;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json.Linq;
using Xunit;

public class UserTests : IClassFixture<WebApplicationFactory<FlapKapBackendChallenge.Program>>
{
    private readonly HttpClient _client;

    public UserTests(WebApplicationFactory<FlapKapBackendChallenge.Program> factory)
    {
        _client = factory.CreateClient();
    }

    private async Task<string> GetJwtTokenAsync(string username = "testbuyer", string password = "123456")
    {
        var loginPayload = new
        {
            userName = username,
            password = password
        };

        var response = await _client.PostAsJsonAsync("/login", loginPayload);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var parsed = JObject.Parse(json);
        return parsed["data"]?["token"]?.ToString() ?? throw new Exception("Token not found");
    }

    [Fact]
    public async Task GetUser_ById_ShouldReturnUser()
    {
        // Act
        var response = await _client.GetAsync("/user/1");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("testbuyer", content);
    }

    [Fact]
    public async Task GetUser_CurrentAuthenticated_ShouldReturnUserInfo()
    {
        // Arrange
        var token = await GetJwtTokenAsync();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Act
        var response = await _client.GetAsync("/user");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("testbuyer", content.ToLower());
    }
}
