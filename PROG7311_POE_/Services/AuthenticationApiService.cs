using Microsoft.AspNetCore.Identity.Data;

namespace PROG7311_POE_.Services
{
    public class AuthenticationApiService
    {
        private readonly HttpClient _httpClient;

        public AuthenticationApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string?> LoginAsync(string username, string password)
        {
            var response = await _httpClient.PostAsJsonAsync("api/authentication/login",
                new
                {
                    username,
                    password
                });

            if (!response.IsSuccessStatusCode)
                return null;

            var result = await response.Content.ReadFromJsonAsync<LoginResponse>();

            return result?.Token;
        }
    }

    public class LoginResponse
    {
        public string Token { get; set; }
    }
}
