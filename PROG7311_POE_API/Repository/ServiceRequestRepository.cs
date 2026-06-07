using Microsoft.EntityFrameworkCore;
using PROG7311_POE_.Data;
using PROG7311_POE_.Models;

namespace PROG7311_POE_API.Repository
{
    public class ServiceRequestRepository : IServiceRequestRepository
    {
        private readonly PROG7311_POE_Context _context;

        public ServiceRequestRepository(PROG7311_POE_Context context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ServiceRequest>> GetAllAsync()
        {
            return await _context.ServiceRequest.ToListAsync();
        }

        public async Task<ServiceRequest?> GetByIdAsync(int id)
        {
            return await _context.ServiceRequest.FirstOrDefaultAsync(c => c.ServiceRequestID == id);

        }

        public async Task CreateAsync(ServiceRequest serviceRequest)
        {
            _context.ServiceRequest.Add(serviceRequest);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ServiceRequest serviceRequest)
        {
            _context.ServiceRequest.Update(serviceRequest);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var serviceRequest = await _context.ServiceRequest.FindAsync(id);
            if (serviceRequest != null)
            {
                _context.ServiceRequest.Remove(serviceRequest);
                await _context.SaveChangesAsync();
            }
        }
    }
}
