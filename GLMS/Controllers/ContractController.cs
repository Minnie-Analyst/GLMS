using GLMS.Data;
using GLMS.Models;
using GLMS.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

public class ContractController : Controller
{
    private readonly ContractService _contractService;
    private readonly FileService _fileService;
    private readonly AppDbContext _context;

    public ContractController(
        AppDbContext context,
        FileService fileService,
        ContractService contractService)
    {
        _context = context;
        _fileService = fileService;
        _contractService = contractService;
    }

    // GET: CONTRACTS
    public async Task<IActionResult> Index(
        DateTime? startDate,
        DateTime? endDate,
        string? status)
    {
        var contracts = await _contractService
            .FilterContractsAsync(
                startDate,
                endDate,
                status);

        return View(contracts);
    }

    // GET: CREATE
    public IActionResult Create()
    {
        LoadClients();
        return View();
    }

    // POST: CREATE
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        Contract contract,
        IFormFile? file)
    {
        // PDF UPLOAD
        contract.FilePath =
            await _fileService.SavePdfAsync(file);

        // INVALID FILE TYPE
        if (file != null && contract.FilePath == null)
        {
            ModelState.AddModelError(
                "",
                "Only PDF files are allowed.");
        }

        // VALIDATION FAIL
        if (!ModelState.IsValid)
        {
            LoadClients(contract.ClientId);
            return View(contract);
        }

        // DEFAULT STATUS
        if (string.IsNullOrEmpty(contract.Status))
        {
            contract.Status = "Active";
        }

        // SAVE
        await _contractService
            .CreateContractAsync(contract);

        TempData["Success"] =
            "Contract created successfully.";

        return RedirectToAction(nameof(Index));
    }

    // GET: EDIT
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var contract =
            await _context.Contracts.FindAsync(id);

        if (contract == null)
        {
            return NotFound();
        }

        LoadClients(contract.ClientId);

        return View(contract);
    }

    // POST: EDIT
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int id,
        Contract contract,
        IFormFile? file)
    {
        if (id != contract.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            LoadClients(contract.ClientId);
            return View(contract);
        }

        var existingContract =
            await _context.Contracts
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);

        if (existingContract == null)
        {
            return NotFound();
        }

        // HANDLE PDF
        if (file != null)
        {
            contract.FilePath =
                await _fileService.SavePdfAsync(file);
        }
        else
        {
            contract.FilePath =
                existingContract.FilePath;
        }

        _context.Update(contract);

        await _context.SaveChangesAsync();

        TempData["Success"] =
            "Contract updated successfully.";

        return RedirectToAction(nameof(Index));
    }

    // GET: DELETE
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var contract = await _context.Contracts
            .Include(c => c.Client)
            .Include(c => c.ServiceRequests)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (contract == null)
        {
            return NotFound();
        }

        return View(contract);
    }

    // POST: DELETE
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var contract = await _context.Contracts
            .Include(c => c.ServiceRequests)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (contract == null)
        {
            return NotFound();
        }

        // BLOCK DELETE IF SERVICE REQUESTS EXIST
        if (contract.ServiceRequests.Any())
        {
            TempData["Error"] =
                "Cannot delete contract with linked service requests.";

            return RedirectToAction(nameof(Index));
        }

        _context.Contracts.Remove(contract);

        await _context.SaveChangesAsync();

        TempData["Success"] =
            "Contract deleted successfully.";

        return RedirectToAction(nameof(Index));
    }

    // DOWNLOAD PDF
    public IActionResult Download(int id)
    {
        var contract = _context.Contracts.Find(id);

        if (contract == null ||
            string.IsNullOrEmpty(contract.FilePath))
        {
            return NotFound();
        }

        var path = Path.Combine(
            Directory.GetCurrentDirectory(),
            "wwwroot",
            contract.FilePath.TrimStart('/'));

        return PhysicalFile(
            path,
            "application/pdf",
            Path.GetFileName(path));
    }

    // CLIENT DROPDOWN
    private void LoadClients(object? selectedClient = null)
    {
        ViewBag.ClientId = new SelectList(
            _context.Clients.Select(c => new
            {
                Id = c.Id,
                Display = c.Name + " (ID: " + c.Id + ")"
            }),
            "Id",
            "Display",
            selectedClient
        );
    }
}