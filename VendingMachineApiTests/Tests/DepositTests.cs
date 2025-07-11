using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FlapKapBackendChallenge;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using Newtonsoft.Json.Linq;

public class DepositTests : IClassFixture<WebApplicationFactory<FlapKapBackendChallenge.Program>>
{
    private readonly HttpClient _client;

    public DepositTests(WebApplicationFactory<FlapKapBackendChallenge.Program> factory)
    {
        _client = factory.CreateClient();
    }
private async Task<string> GetJwtTokenAsync()
    {
        var loginPayload = new
        {
            userName = "testbuyer",
            password = "123456"
        };

        var response = await _client.PostAsJsonAsync("/login", loginPayload);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var parsed = JObject.Parse(json);
        var token = parsed["data"]?["token"]?.ToString();

        return token ?? throw new Exception("JWT token not found in login response.");
    }
    [Fact]
    public async Task Deposit_WithValidToken_ShouldSucceed()
    {
        var token = await GetJwtTokenAsync();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var depositPayload = new { amount = 100 };

        var response = await _client.PostAsJsonAsync("/deposit", depositPayload);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
    [Fact]
    public async Task Deposit_InvalidAmount_ShouldFail()
    {
        var token = await GetJwtTokenAsync();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var depositPayload = new { amount = 3 }; // Invalid coin
        var response = await _client.PostAsJsonAsync("/deposit", depositPayload);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

}
