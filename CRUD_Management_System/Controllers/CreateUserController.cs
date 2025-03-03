using CRUD_Management_System.Data;
using CRUD_Management_System.Models;
using CRUD_Management_System.Services; // Vergeet niet om de juiste namespace van de service toe te voegen
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Threading.Tasks;

public class CreateUserController : Controller
{
    private readonly AppDbContext _context;
    private readonly AliasService _aliasService;

    // Dependency Injection van de AliasService in de controller
    public CreateUserController(AppDbContext context, AliasService aliasService)
    {
        _context = context;
        _aliasService = aliasService;
    }

    // Toon het formulier voor het maken van een nieuwe gebruiker
    public IActionResult Index()
    {
        return View();
    }

    // Verwerk het formulier om een nieuwe gebruiker op te slaan
    [HttpPost]
    public async Task<IActionResult> CreateUser(UserDetailsModel newUser)
    {
        // Genereer de alias voor de nieuwe gebruiker via de service
        newUser.Alias = await _aliasService.CreateTXTAlias(newUser.Name, newUser.Surname);

        // Debug output voor de nieuwe alias
        Debug.WriteLine($"New Alias: {newUser.Alias}");

        if (!ModelState.IsValid)
        {
            // Foutmeldingen tonen
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Debug.WriteLine(error.ErrorMessage);
            }

            TempData["ErrorMessage"] = "Er is een fout opgetreden bij het aanmaken van de gebruiker.";
            return View("Index", newUser); // Formulier opnieuw tonen bij fouten
        }

        // Voeg de nieuwe gebruiker toe aan de database
        _context.UserDetails.Add(newUser);
        await _context.SaveChangesAsync();

        // Redirect naar dashboard
        return RedirectToAction("Index", "DashboardAdmin");
    }

    [HttpPost]
    public async Task<IActionResult> GenerateAlias(string name, string surname)
    {
        var alias = await _aliasService.CreateTXTAlias(name, surname);
        return Json(alias);
    }
}
