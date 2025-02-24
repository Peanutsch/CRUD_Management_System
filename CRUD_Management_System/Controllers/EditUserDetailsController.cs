using CRUD_Management_System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class EditUserDetailsController : Controller
{
    private readonly AppDbContext _context;

    public EditUserDetailsController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var users = await _context.UserDetails.Skip(3).ToListAsync();
        ViewData["CurrentUser"] = TempData["CurrentUser"]; // Haal de waarde uit TempData
        return View(users);
    }

    // Een actie voor het bekijken van details van een specifieke gebruiker
    public async Task<IActionResult> Details(string aliasId)
    {
        if (aliasId == null)
        {
            return NotFound();
        }

        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.AliasId == aliasId);

        if (user == null)
        {
            return NotFound();
        }

        return View(user);
    }
}
