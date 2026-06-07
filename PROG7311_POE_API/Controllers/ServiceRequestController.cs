using Microsoft.AspNetCore.Mvc;
using PROG7311_POE_.Models;
using PROG7311_POE_API.Services;

namespace PROG7311_POE_API.Controllers
{
    [ApiController]
    [Route("api/servicerequests")]
    public class ServiceRequestController : Controller
    {
        private readonly IServiceRequestService _service;

        public ServiceRequestController(IServiceRequestService service)
        {
            _service = service;
        }

        // GET: api/servicerequests
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetAllServiceRequestsAsync());
        }

        // GET: api/servicerequests/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var client = await _service.GetServiceRequestByIdAsync(id);

            if (client == null)
                return NotFound();

            return Ok(client);
        }

        // POST: api/servicerequests
        [HttpPost]
        public async Task<IActionResult> CreateServiceRequest([FromBody] ServiceRequest serviceRequest)
        {
            await _service.CreateServiceRequestAsync(serviceRequest);

            return CreatedAtAction(nameof(GetById),new { id = serviceRequest.ServiceRequestID }, serviceRequest);
        }

        // PUT: api/servicerequests/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateServiceRequest(int id, [FromBody] ServiceRequest serviceRequest)
        {
            if (id != serviceRequest.ServiceRequestID)
                return BadRequest("ID mismatch");

            var existing = await _service.GetServiceRequestByIdAsync(id);

            if (existing == null)
                return NotFound();

            await _service.UpdateServiceRequestAsync(serviceRequest);

            return Ok(serviceRequest);
        }

        // DELETE: api/servicerequests/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteServiceRequest(int id)
        {
            var existing = await _service.GetServiceRequestByIdAsync(id);

            if (existing == null)
                return NotFound();

            await _service.DeleteServiceRequestAsync(id);

            return NoContent();
        }
    }
}
