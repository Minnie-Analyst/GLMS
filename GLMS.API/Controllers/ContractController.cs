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
    public class ContractsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ContractsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/contracts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Contract>>> GetContracts()
        {
            return await _context.Contracts
                .Include(c => c.Client)
                .ToListAsync();
        }

        // POST: api/contracts
        [HttpPost]
        public async Task<ActionResult<Contract>> CreateContract(Contract contract)
        {
            if (contract == null)
            {
                return BadRequest();
            }

            _context.Contracts.Add(contract);

            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetContracts),
                new { id = contract.Id },
                contract);
        }
        // PATCH: api/contracts/5/status
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(
            int id,
            UpdateContractStatusDto dto)
        {
            var contract = await _context.Contracts.FindAsync(id);

            if (contract == null)
            {
                return NotFound();
            }

            contract.Status = dto.Status;

            await _context.SaveChangesAsync();

            return Ok(contract);
        }
        // PUT: api/contracts/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateContract(
            int id,
            Contract contract)
        {
            if (id != contract.Id)
            {
                return BadRequest();
            }

            _context.Entry(contract).State =
                EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Contracts.Any(c => c.Id == id))
                {
                    return NotFound();
                }

                throw;
            }

            return NoContent();
        }

        // DELETE: api/contracts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContract(int id)
        {
            var contract =
                await _context.Contracts.FindAsync(id);

            if (contract == null)
            {
                return NotFound();
            }

            _context.Contracts.Remove(contract);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}