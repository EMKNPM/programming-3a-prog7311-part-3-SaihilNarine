using Microsoft.EntityFrameworkCore;
using PROG7311_POE_.Data;
using PROG7311_POE_.Models;

namespace PROG7311_POE_.API.Repository
{
    public class ContractRepository : IContractRepository
    {
        private readonly PROG7311_POE_Context _context;

        public ContractRepository(PROG7311_POE_Context context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Contract>> GetAllAsync()
        {
            return await _context.Contract.ToListAsync();
        }

        public async Task<Contract?> GetByIdAsync(int id)
        {
            return await _context.Contract.FirstOrDefaultAsync(c => c.ContractID == id);

        }

        public async Task CreateAsync(Contract contract)
        {
            _context.Contract.Add(contract);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Contract contract)
        {
            _context.Contract.Update(contract);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var contract = await _context.Contract.FindAsync(id);
            if (contract != null)
            {
                _context.Contract.Remove(contract);
                await _context.SaveChangesAsync();
            }
        }
    }
}