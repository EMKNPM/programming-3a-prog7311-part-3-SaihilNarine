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

        private void SetToken(string token)
        {
            if (string.IsNullOrEmpty(token))
                throw new Exception("JWT token is missing");

            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }

        // GET ALL
        public async Task<List<ServiceRequest>> GetServiceRequestsAsync(string token)
        {
            SetToken(token);

            var response = await _httpClient.GetAsync("api/servicerequests");

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"GET FAILED: {response.StatusCode} - {error}");
            }

            return await response.Content.ReadFromJsonAsync<List<ServiceRequest>>()
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
        public async Task CreateServiceRequestAsync(ServiceRequest serviceRequest, string token)
        {
            SetToken(token);

            var response = await _httpClient.PostAsJsonAsync("api/servicerequests", serviceRequest);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"CREATE FAILED: {response.StatusCode} - {error}");
            }
        }

        // UPDATE
        public async Task UpdateServiceRequestAsync(ServiceRequest serviceRequest, string token)
        {
            SetToken(token);

            var response = await _httpClient.PutAsJsonAsync(
                $"api/servicerequests/{serviceRequest.ServiceRequestID}",
                serviceRequest
            );

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"UPDATE FAILED: {response.StatusCode} - {error}");
            }
        }

        // DELETE
        public async Task DeleteServiceRequestAsync(int id, string token)
        {
            SetToken(token);

            var response = await _httpClient.DeleteAsync($"api/servicerequests/{id}");

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"DELETE FAILED: {response.StatusCode} - {error}");
            }
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