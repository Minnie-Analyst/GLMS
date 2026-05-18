using GLMS.Data;
using GLMS.Models;
using GLMS.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

public class ServiceRequestController : Controller
{
    private readonly AppDbContext _context;
    private readonly CurrencyService _currencyService;

    public ServiceRequestController(
        AppDbContext context,
        CurrencyService currencyService)
    {
        _context = context;
        _currencyService = currencyService;
    }

    // INDEX
    public async Task<IActionResult> Index()
    {
        var requests = await _context.ServiceRequests
            .Include(s => s.Contract)
            .ThenInclude(c => c.Client)
            .ToListAsync();

        return View(requests);
    }

    // GET: CREATE
    public IActionResult Create()
    {
        LoadContracts();
        return View();
    }

    // POST: CREATE
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ServiceRequest request)
    {
        var contract = await _context.Contracts
            .FindAsync(request.ContractId);

        // BLOCK INVALID CONTRACTS
        if (contract == null ||
            contract.Status == "Expired" ||
            contract.Status == "On Hold")
        {
            ModelState.AddModelError(
                "",
                "Cannot create request for expired or on-hold contract.");
        }

        // GET EXCHANGE RATE
        var rate =
            await _currencyService.GetUsdToZarRate();

        request.CostZAR =
            request.CostUSD * rate;

        // DEFAULT STATUS
        if (string.IsNullOrEmpty(request.Status))
        {
            request.Status = "Pending";
        }

        if (!ModelState.IsValid)
        {
            LoadContracts(request.ContractId);
            return View(request);
        }

        _context.ServiceRequests.Add(request);

        await _context.SaveChangesAsync();

        TempData["Success"] =
            "Service request created successfully.";

        return RedirectToAction(nameof(Index));
    }

    // GET: EDIT
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var request = await _context.ServiceRequests
            .FindAsync(id);

        if (request == null)
        {
            return NotFound();
        }

        LoadContracts(request.ContractId);

        return View(request);
    }

    // POST: EDIT
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int id,
        ServiceRequest request)
    {
        if (id != request.Id)
        {
            return NotFound();
        }

        var contract = await _context.Contracts
            .FindAsync(request.ContractId);

        // BLOCK INVALID CONTRACTS
        if (contract == null ||
            contract.Status == "Expired" ||
            contract.Status == "On Hold")
        {
            ModelState.AddModelError(
                "",
                "Cannot assign request to expired or on-hold contract.");
        }

        // UPDATE EXCHANGE
        var rate =
            await _currencyService.GetUsdToZarRate();

        request.CostZAR =
            request.CostUSD * rate;

        if (!ModelState.IsValid)
        {
            LoadContracts(request.ContractId);
            return View(request);
        }

        _context.Update(request);

        await _context.SaveChangesAsync();

        TempData["Success"] =
            "Service request updated successfully.";

        return RedirectToAction(nameof(Index));
    }

    // GET: DELETE
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var request = await _context.ServiceRequests
            .Include(s => s.Contract)
            .ThenInclude(c => c.Client)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (request == null)
        {
            return NotFound();
        }

        return View(request);
    }

    // POST: DELETE
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var request = await _context.ServiceRequests
            .FindAsync(id);

        if (request == null)
        {
            return NotFound();
        }

        _context.ServiceRequests.Remove(request);

        await _context.SaveChangesAsync();

        TempData["Success"] =
            "Service request deleted successfully.";

        return RedirectToAction(nameof(Index));
    }

    // LOAD CONTRACTS
    private void LoadContracts(object? selected = null)
    {
        ViewBag.ContractId = new SelectList(
            _context.Contracts
                .Include(c => c.Client)
                .Select(c => new
                {
                    Id = c.Id,
                    Display =
                        "Contract " + c.Id +
                        " - " + c.Client.Name
                }),
            "Id",
            "Display",
            selected
        );
    }
}