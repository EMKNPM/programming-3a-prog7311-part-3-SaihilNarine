using Microsoft.AspNetCore.Mvc;
using PROG7311_POE_.API.Repository;
using PROG7311_POE_.Models;
using PROG7311_POE_API.Services;

namespace PROG7311_POE_.API.Controllers
{
    [ApiController]
    [Route("api/contracts")]
    public class ContractsController : ControllerBase
    {
        private readonly IContractService _service;

        public ContractsController(IContractService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetAllContractsAsync());
        }

        // GET Contract By ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var contract = await _service.GetContractByIdAsync(id);

            if (contract == null)
                return NotFound();

            return Ok(contract);
        }

        // POST
        [HttpPost]
        public async Task<IActionResult> CreateContract(
            [FromBody] Contract contract)
        {
            await _service.CreateContractAsync(contract);
            return CreatedAtAction(nameof(GetById),
                new { id = contract.ContractID },
                contract);
        }

        // PATCH 
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(
            int id,
            string status)
        {
            var contract = await _service.GetContractByIdAsync(id);

            if (contract == null)
                return NotFound();

            contract.Status = status;

            await _service.UpdateContractAsync(contract);

            return Ok(contract);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContract(int id)
        {
            var contract = await _service.GetContractByIdAsync(id);

            if (contract == null)
            {
                return NotFound();
            }

            await _service.DeleteContractsAsync(id);

            return NoContent();
        }
    }
}