using PROG7311_POE_.Models;

namespace PROG7311_POE_.API.Repository
{
    public interface IContractRepository
    {
        Task<IEnumerable<Contract>> GetAllAsync();
        Task<Contract?> GetByIdAsync(int id);
        Task CreateAsync(Contract contract);
        Task UpdateAsync(Contract contract);
        Task DeleteAsync(int id);

    }
}