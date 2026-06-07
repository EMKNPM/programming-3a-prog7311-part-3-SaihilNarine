using PROG7311_POE_.API.Repository;
using PROG7311_POE_.Models;
using PROG7311_POE_API.Repository;

namespace PROG7311_POE_API.Services
{
    public class ServiceRequestService : IServiceRequestService
    {
        private readonly IServiceRequestRepository _repository;

        public ServiceRequestService(IServiceRequestRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ServiceRequest>> GetAllServiceRequestsAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<ServiceRequest?> GetServiceRequestByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task CreateServiceRequestAsync(ServiceRequest serviceRequest)
        {
            await _repository.CreateAsync(serviceRequest);
        }

        public async Task UpdateServiceRequestAsync(ServiceRequest serviceRequest)
        {
            await _repository.UpdateAsync(serviceRequest);
        }

        public async Task DeleteServiceRequestAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
