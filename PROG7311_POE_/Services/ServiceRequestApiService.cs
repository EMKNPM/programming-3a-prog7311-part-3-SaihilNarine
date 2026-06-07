using PROG7311_POE_.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace PROG7311_POE_.Services
{
    public class ServiceRequestApiService
    {
        private readonly HttpClient _httpClient;

        public ServiceRequestApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // SET TOKEN ONCE
        private void SetToken(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        // GET ALL
        public async Task<List<ServiceRequest>> GetServiceRequestsAsync(string token)
        {
            SetToken(token);

            return await _httpClient.GetFromJsonAsync<List<ServiceRequest>>("api/servicerequests")
            ?? new List<ServiceRequest>();
        }

        // GET BY ID
        public async Task<ServiceRequest?> GetServiceRequestByIdAsync(int id, string token)
        {
            SetToken(token);

            var response = await _httpClient.GetAsync($"api/servicerequests/{id}");

            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<ServiceRequest>(json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
        }

        // CREATE
        public async Task<bool> CreateServiceRequestAsync(ServiceRequest serviceRequest, string token)
        {
            SetToken(token);

            var response = await _httpClient.PostAsJsonAsync("api/servicerequests", serviceRequest);

            return response.IsSuccessStatusCode;
        }

        // UPDATE
        public async Task<bool> UpdateServiceRequestAsync(ServiceRequest serviceRequest, string token)
        {
            SetToken(token);

            var response = await _httpClient.PutAsJsonAsync($"api/servicerequests/{serviceRequest.ServiceRequestID}", serviceRequest);

            return response.IsSuccessStatusCode;
        }

        // DELETE
        public async Task<bool> DeleteServiceRequestAsync(int id, string token)
        {
            SetToken(token);

            var response = await _httpClient.DeleteAsync($"api/servicerequests/{id}");

            return response.IsSuccessStatusCode;
        }

        // EXISTS CHECK
        public async Task<bool> ServiceRequestExistsAsync(int id, string token)
        {
            SetToken(token);

            var response = await _httpClient.GetAsync($"api/servicerequests/{id}");

            return response.IsSuccessStatusCode;
        }
    }
}