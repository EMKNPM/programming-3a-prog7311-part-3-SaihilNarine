using PROG7311_POE_.API.Repository;
using PROG7311_POE_.Models;

namespace PROG7311_POE_API.Services
{
    public class ContractService : IContractService
    {
        private readonly IContractRepository _repository;

        public ContractService(IContractRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Contract>>GetAllContractsAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Contract?>GetContractByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task CreateContractAsync(Contract contract)
        {
            await _repository.CreateAsync(contract);
        }

        public async Task UpdateContractAsync(Contract contract)
        {
            await _repository.UpdateAsync(contract);
        }

        public async Task DeleteContractsAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}