using PROG7311_POE_.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace PROG7311_POE_.Services
{
    public class ContractApiService
    {
        private readonly HttpClient _httpClient;

        public ContractApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // GET ALL
        public async Task<List<Contract>> GetContractsAsync(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            
            return await _httpClient.GetFromJsonAsync<List<Contract>>("api/contracts") ?? new List<Contract>();
        }

        // GET BY ID
        public async Task<Contract?> GetContractByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"api/contracts/{id}");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var json = await response.Content.ReadAsStringAsync();

            // DEBUG
            Console.WriteLine(json);

            return JsonSerializer.Deserialize<Contract>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }

        // CREATE
        public async Task<bool> CreateContractAsync(Contract contract)
        {
            var response = await _httpClient.PostAsJsonAsync("api/contracts", contract);
            return response.IsSuccessStatusCode;
        }

        // UPDATE
        public async Task<bool> UpdateContractAsync(Contract contract)
        {
            var response = await _httpClient.PutAsJsonAsync(
                $"api/contracts/{contract.ContractID}",
                contract);

            return response.IsSuccessStatusCode;
        }

        // DELETE
        public async Task<bool> DeleteContractAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/contracts/{id}");
            return response.IsSuccessStatusCode;
        }

        // PATCH STATUS
        public async Task<bool> UpdateStatusAsync(int id, string status)
        {
            var response = await _httpClient.PatchAsync(
                $"api/contracts/{id}/status?status={status}",
                null);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> ContractExistsAsync(int id)
        {
            var contract = await _httpClient.GetAsync($"api/contracts/{id}");
            return contract.IsSuccessStatusCode;
        }
    }
}