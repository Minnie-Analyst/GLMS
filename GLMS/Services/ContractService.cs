using GLMS.Data;
using GLMS.Models;
using Microsoft.EntityFrameworkCore;

namespace GLMS.Services;

public class ContractService
{
    private readonly AppDbContext _context;

    public ContractService(AppDbContext context)
    {
        _context = context;
    }

    // GET ALL CONTRACTS
    public async Task<List<Contract>> GetAllContractsAsync()
    {
        return await _context.Contracts
            .Include(c => c.Client)
            .ToListAsync();
    }

    // FILTER CONTRACTS
    public async Task<List<Contract>> FilterContractsAsync(
        DateTime? startDate,
        DateTime? endDate,
        string? status)
    {
        var contracts = _context.Contracts
            .Include(c => c.Client)
            .AsQueryable();

        if (startDate.HasValue)
            contracts = contracts.Where(c =>
                c.StartDate >= startDate.Value);

        if (endDate.HasValue)
            contracts = contracts.Where(c =>
                c.EndDate <= endDate.Value);

        if (!string.IsNullOrEmpty(status))
            contracts = contracts.Where(c =>
                c.Status == status);

        return await contracts.ToListAsync();
    }

    // CREATE CONTRACT
    public async Task CreateContractAsync(Contract contract)
    {
        _context.Contracts.Add(contract);
        await _context.SaveChangesAsync();
    }

    // GET CONTRACT BY ID
    public async Task<Contract?> GetByIdAsync(int id)
    {
        return await _context.Contracts
            .Include(c => c.Client)
            .FirstOrDefaultAsync(c => c.Id == id);
    }
}