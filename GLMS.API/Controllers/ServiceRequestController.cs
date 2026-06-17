using GLMS.API.Data;
using GLMS.API.Models;
using GLMS.API.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace GLMS.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceRequestsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ServiceRequestsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/ServiceRequests
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ServiceRequest>>> GetServiceRequests()
        {
            return await _context.ServiceRequests
                .Include(sr => sr.Contract)
                .ToListAsync();
        }

        // GET: api/ServiceRequests/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceRequest>> GetServiceRequest(int id)
        {
            var request = await _context.ServiceRequests
                .Include(sr => sr.Contract)
                .FirstOrDefaultAsync(sr => sr.Id == id);

            if (request == null)
            {
                return NotFound();
            }

            return request;
        }

        // POST: api/ServiceRequests
        [HttpPost]
        public async Task<ActionResult<ServiceRequest>> CreateServiceRequest(
            ServiceRequest request)
        {
            _context.ServiceRequests.Add(request);

            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetServiceRequest),
                new { id = request.Id },
                request);
        }

        // PATCH: api/ServiceRequests/5/status
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(
            int id,
            ServiceRequestStatusDto dto)
        {
            var request =
                await _context.ServiceRequests.FindAsync(id);

            if (request == null)
            {
                return NotFound();
            }

            request.Status = dto.Status;

            await _context.SaveChangesAsync();

            return Ok(request);
        }
        // PUT: api/ServiceRequests/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateServiceRequest(
            int id,
            ServiceRequest request)
        {
            if (id != request.Id)
            {
                return BadRequest();
            }

            _context.Entry(request).State =
                EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.ServiceRequests.Any(
                    sr => sr.Id == id))
                {
                    return NotFound();
                }

                throw;
            }

            return NoContent();
        }

        // DELETE: api/ServiceRequests/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteServiceRequest(
            int id)
        {
            var request =
                await _context.ServiceRequests.FindAsync(id);

            if (request == null)
            {
                return NotFound();
            }

            _context.ServiceRequests.Remove(request);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}