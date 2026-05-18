using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GLMS.Data;
using GLMS.Models;

public class ClientController : Controller
{
    private readonly AppDbContext _context;

    public ClientController(AppDbContext context)
    {
        _context = context;
    }

    // GET: Client
    public async Task<IActionResult> Index()
    {
        return View(await _context.Clients.ToListAsync());
    }
    // GET: EDIT
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var client = await _context.Clients.FindAsync(id);

        if (client == null)
        {
            return NotFound();
        }

        return View(client);
    }
    // POST: EDIT
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int id,
        Client client)
    {
        if (id != client.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            _context.Update(client);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        return View(client);
    }
    // GET: DELETE
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var client = await _context.Clients
            .Include(c => c.Contracts)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (client == null)
        {
            return NotFound();
        }

        return View(client);
    }
    // POST: DELETE
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var client = await _context.Clients
            .Include(c => c.Contracts)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (client == null)
        {
            return NotFound();
        }

        // BLOCK DELETE IF CLIENT HAS CONTRACTS
        if (client.Contracts.Any())
        {
            TempData["Error"] =
                "Cannot delete client with linked contracts.";

            return RedirectToAction(nameof(Index));
        }

        _context.Clients.Remove(client);

        await _context.SaveChangesAsync();

        TempData["Success"] =
            "Client deleted successfully.";

        return RedirectToAction(nameof(Index));
    }
    // GET: Client/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Client/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Client client)
    {
        if (ModelState.IsValid)
        {
            _context.Add(client);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index)); //  IMPORTANT
        }

        return View(client); //  shows validation errors
    }
}