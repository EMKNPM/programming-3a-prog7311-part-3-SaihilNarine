using PROG7311_POE_.Models;
using System.Net;
using System.Net.Http.Json;
using System.Net.Http.Headers;
using Xunit;

namespace PROG7311_POE_.Tests
{
    public class ApiIntegrationTests
    {
        private readonly HttpClient _client;

        public ApiIntegrationTests()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7224/")
            };
        }

        private async Task<string> GetTokenAsync()
        {
            var login = new
            {
                Username = "admin",
                Password = "1234"
            };

            var response = await _client.PostAsJsonAsync(
                "api/authentication/login",
                login);

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<AuthResponse>();

            return result!.Token;
        }

        private void SetToken(string token)
        {
            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }

        public class AuthResponse
        {
            public string Token { get; set; } = "";
        }

        [Fact]
        public async Task GetClients_ShouldReturnSuccess()
        {
            var token = await GetTokenAsync();
            SetToken(token);

            var response = await _client.GetAsync("api/clients");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task CreateClient_ShouldReturnCreated()
        {
            var token = await GetTokenAsync();
            SetToken(token);

            var client = new Client
            {
                Name = "Integration Test Client",
                ContactDetails = "0821234567",
                Region = "Durban"
            };

            var response = await _client.PostAsJsonAsync("api/clients", client);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }


        [Fact]
        public async Task GetContracts_ShouldReturnSuccess()
        {
            var token = await GetTokenAsync();
            SetToken(token);

            var response = await _client.GetAsync("api/contracts");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }


        [Fact]
        public async Task GetServiceRequests_ShouldReturnSuccess()
        {
            var token = await GetTokenAsync();
            SetToken(token);

            var response = await _client.GetAsync("api/servicerequests");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}