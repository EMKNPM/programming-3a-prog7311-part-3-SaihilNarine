using Microsoft.AspNetCore.Mvc;
using PROG7311_POE_.Models;
using PROG7311_POE_API.Services;

namespace PROG7311_POE_API.Controllers
{
    [ApiController]
    [Route("api/clients")]
    public class ClientsController : ControllerBase
    {
        private readonly IClientService _service;

        public ClientsController(IClientService service)
        {
            _service = service;
        }

        // GET: api/clients
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetAllClientsAsync());
        }

        // GET: api/clients/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var client = await _service.GetClientByIdAsync(id);

            if (client == null)
                return NotFound();

            return Ok(client);
        }

        // POST: api/clients
        [HttpPost]
        public async Task<IActionResult> CreateClient([FromBody] Client client)
        {
            await _service.CreateClientAsync(client);

            return CreatedAtAction(
                nameof(GetById),
                new { id = client.ClientID },
                client);
        }

        // PUT: api/clients/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateClient(int id, [FromBody] Client client)
        {
            if (id != client.ClientID)
                return BadRequest("ID mismatch");

            var existing = await _service.GetClientByIdAsync(id);

            if (existing == null)
                return NotFound();

            await _service.UpdateClientAsync(client);

            return Ok(client);
        }

        // DELETE: api/clients/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClient(int id)
        {
            var existing = await _service.GetClientByIdAsync(id);

            if (existing == null)
                return NotFound();

            await _service.DeleteClientAsync(id);

            return NoContent();
        }
    }
}