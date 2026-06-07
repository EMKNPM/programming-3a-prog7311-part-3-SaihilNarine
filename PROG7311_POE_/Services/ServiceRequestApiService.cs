using PROG7311_POE_.Models;
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

        // GET ALL
        public async Task<List<ServiceRequest>> GetServiceRequestsAsync()
        {
            var result = await _httpClient.GetFromJsonAsync<List<ServiceRequest>>("api/servicerequests");
            return result ?? new List<ServiceRequest>();
        }

        // GET BY ID
        public async Task<ServiceRequest?> GetServiceRequestByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"api/servicerequests/{id}");

            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();

            Console.WriteLine(json); // DEBUG

            return JsonSerializer.Deserialize<ServiceRequest>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }

        // CREATE
        public async Task<bool> CreateServiceRequestAsync(ServiceRequest serviceRequest)
        {
            var response = await _httpClient.PostAsJsonAsync("api/servicerequests", serviceRequest);
            return response.IsSuccessStatusCode;
        }

        // UPDATE
        public async Task<bool> UpdateServiceRequestAsync(ServiceRequest serviceRequest)
        {
            var response = await _httpClient.PutAsJsonAsync(
                $"api/servicerequests/{serviceRequest.ServiceRequestID}",
                serviceRequest);

            return response.IsSuccessStatusCode;
        }

        // DELETE
        public async Task<bool> DeleteServiceRequestAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/servicerequests/{id}");
            return response.IsSuccessStatusCode;
        }

        // EXISTS CHECK
        public async Task<bool> ServiceRequestExistsAsync(int id)
        {
            var response = await _httpClient.GetAsync($"api/servicerequests/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}