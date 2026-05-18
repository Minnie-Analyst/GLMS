using Microsoft.AspNetCore.Mvc;
using GLMS.Data;

public class DashboardController : Controller
{
    private readonly AppDbContext _context;

    public DashboardController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        ViewBag.TotalClients =
            _context.Clients.Count();

        ViewBag.TotalContracts =
            _context.Contracts.Count();

        ViewBag.ActiveContracts =
            _context.Contracts.Count(c =>
                c.Status == "Active");

        ViewBag.ExpiredContracts =
            _context.Contracts.Count(c =>
                c.Status == "Expired");

        ViewBag.TotalRequests =
            _context.ServiceRequests.Count();

        ViewBag.PendingRequests =
            _context.ServiceRequests.Count(s =>
                s.Status == "Pending");

        ViewBag.TotalRevenue =
            _context.ServiceRequests.Sum(s =>
                (decimal?)s.CostZAR) ?? 0;

        return View();
    }
}