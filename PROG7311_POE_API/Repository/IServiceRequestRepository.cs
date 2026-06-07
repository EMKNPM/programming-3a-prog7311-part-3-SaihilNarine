using PROG7311_POE_.Models;

namespace PROG7311_POE_API.Repository
{
    public interface IServiceRequestRepository
    {
        Task<IEnumerable<ServiceRequest>> GetAllAsync();
        Task<ServiceRequest?> GetByIdAsync(int id);
        Task CreateAsync(ServiceRequest servicerequest);
        Task UpdateAsync(ServiceRequest servicerequest);
        Task DeleteAsync(int id);
    }
}
