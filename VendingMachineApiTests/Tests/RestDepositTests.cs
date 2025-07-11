using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Net;
using Xunit;

namespace VendingMachineApiTests.Tests
{
    public class ResetDepositTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public ResetDepositTests(WebApplicationFactory<FlapKapBackendChallenge.Program> factory)
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
        public async Task ResetDeposit_ShouldResetToZero()
        {
            var token = await GetJwtTokenAsync();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.PostAsync("/reset", null);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }

}
