using Microsoft.AspNetCore.Mvc;
using CRUD_Management_System.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CRUD_Management_System.Data;

public class LoginController : Controller
{
    private readonly AppDbContext _context;

    public LoginController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Index(LoginModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.AliasId == model.AliasId && u.Password == model.Password);

        if (user != null)
        {
            // Login succesvol, redirect naar dashboard
            return RedirectToAction("Index", "DashboardAdmin");
        }

        ModelState.AddModelError("", "Ongeldige inloggegevens");
        return View(model);
    }
}
