using PROG7311_POE_.API.Repository;
using PROG7311_POE_.Models;
using PROG7311_POE_API.Repository;

namespace PROG7311_POE_API.Services
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _repository;

        public ClientService(IClientRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Client>> GetAllClientsAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Client?> GetClientByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task CreateClientAsync(Client client)
        {
            await _repository.CreateAsync(client);
        }

        public async Task UpdateClientAsync(Client client)
        {
            await _repository.UpdateAsync(client);
        }

        public async Task DeleteClientAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
