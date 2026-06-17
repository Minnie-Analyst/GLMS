using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using Xunit;

namespace GLMS.API.Tests
{
    public class ContractApiTests :
        IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public ContractApiTests(
            WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetContracts_ReturnsResponse()
        {
            var response =
                await _client.GetAsync("/api/contracts");

            Assert.True(
                response.StatusCode == HttpStatusCode.OK ||
                response.StatusCode == HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task GetServiceRequests_ReturnsSuccessStatusCode()
        {
            var response =
                await _client.GetAsync("/api/ServiceRequests");

            Assert.Equal(
                HttpStatusCode.OK,
                response.StatusCode);
        }

        [Fact]
        public async Task GetServiceRequests_ReturnsJsonData()
        {
            var response =
                await _client.GetAsync("/api/ServiceRequests");

            response.EnsureSuccessStatusCode();

            var json =
                await response.Content.ReadAsStringAsync();

            Assert.False(string.IsNullOrWhiteSpace(json));
        }
    }
}