using PROG7311_POE_.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace PROG7311_POE_.Services
{
    public class ClientApiService
    {
        private readonly HttpClient _httpClient;

        public ClientApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // GET ALL CLIENTS
        public async Task<List<Client>> GetClientsAsync(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var result = await _httpClient.GetFromJsonAsync<List<Client>>("api/clients");

            return result ?? new List<Client>();
        }

        // GET CLIENT BY ID
        public async Task<Client?> GetClientByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"api/clients/{id}");

            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();

            // DEBUG (optional)
            Console.WriteLine(json);

            return JsonSerializer.Deserialize<Client>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }

        // CREATE CLIENT
        public async Task<bool> CreateClientAsync(Client client)
        {
            var response = await _httpClient.PostAsJsonAsync("api/clients", client);
            return response.IsSuccessStatusCode;
        }

        // UPDATE CLIENT
        public async Task<bool> UpdateClientAsync(Client client)
        {
            var response = await _httpClient.PutAsJsonAsync(
                $"api/clients/{client.ClientID}",
                client);

            return response.IsSuccessStatusCode;
        }

        // DELETE CLIENT
        public async Task<bool> DeleteClientAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/clients/{id}");
            return response.IsSuccessStatusCode;
        }

        // CHECK IF CLIENT EXISTS
        public async Task<bool> ClientExistsAsync(int id)
        {
            var response = await _httpClient.GetAsync($"api/clients/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}