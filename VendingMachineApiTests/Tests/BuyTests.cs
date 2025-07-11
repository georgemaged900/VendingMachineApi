using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Net;
using Xunit;

public class BuyTests : IClassFixture<WebApplicationFactory<FlapKapBackendChallenge.Program>>
{
    private readonly HttpClient _client;

    public BuyTests(WebApplicationFactory<FlapKapBackendChallenge.Program> factory)
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
        return parsed["data"]?["token"]?.ToString() ?? throw new Exception("Token not found.");
    }

    [Fact]
    public async Task Buy_WithSufficientDeposit_ShouldSucceed()
    {
        var token = await GetJwtTokenAsync();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var payload = new { productId = 1, quantity = 1 };
        var response = await _client.PostAsJsonAsync("/buy", payload);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Buy_InsufficientDeposit_ShouldFail()
    {
        var token = await GetJwtTokenAsync();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var payload = new { productId = 1, quantity = 99 };
        var response = await _client.PostAsJsonAsync("/buy", payload);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
