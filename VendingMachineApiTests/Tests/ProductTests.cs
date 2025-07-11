using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FlapKapBackendChallenge;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json.Linq;
using Xunit;

public class ProductTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public ProductTests(WebApplicationFactory<FlapKapBackendChallenge.Program> factory)
    {
        _client = factory.CreateClient();
    }

    private async Task<string> GetTokenAsync(string username, string password)
    {
        var loginPayload = new { userName = username, password = password };

        var response = await _client.PostAsJsonAsync("/login", loginPayload);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var parsed = JObject.Parse(json);
        return parsed["data"]?["token"]?.ToString() ?? throw new Exception("Token not found");
    }

    [Fact]
    public async Task GetAllProducts_ShouldReturnOk()
    {
        var response = await _client.GetAsync("/product");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetProductById_ShouldReturnOk()
    {
        var response = await _client.GetAsync("/product/1");
        Assert.True(response.StatusCode is HttpStatusCode.OK or HttpStatusCode.NotFound); // depends on seeded data
    }

    [Fact]
    public async Task AddProduct_WithSellerRole_ShouldSucceed()
    {
        var token = await GetTokenAsync("testseller", "123456");
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var productPayload = new
        {
            ProductName = "Test Cola",
            AmountAvailable = 20,
            Cost = 50
        };

        var response = await _client.PostAsJsonAsync("/product", productPayload);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task AddProduct_WithBuyerRole_ShouldFail()
    {
        var token = await GetTokenAsync("testbuyer", "123456");
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var productPayload = new
        {
            ProductName = "Invalid Attempt",
            AmountAvailable = 10,
            Cost = 100
        };

        var response = await _client.PostAsJsonAsync("/product", productPayload);
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task UpdateProduct_WithWrongSeller_ShouldFail()
    {
        var token = await GetTokenAsync("testseller", "123456");
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var updatePayload = new
        {
            ProductName = "Updated Product",
            AmountAvailable = 50,
            Cost = 100
        };

        var response = await _client.PutAsJsonAsync("/product/999", updatePayload); // assume 999 doesn't belong to this seller
        Assert.True(response.StatusCode is HttpStatusCode.NotFound or HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task DeleteProduct_WithWrongSeller_ShouldFail()
    {
        var token = await GetTokenAsync("testseller", "123456");
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.DeleteAsync("/product/999"); // assume 999 doesn't belong to this seller
        Assert.True(response.StatusCode is HttpStatusCode.NotFound or HttpStatusCode.Forbidden);
    }
}
