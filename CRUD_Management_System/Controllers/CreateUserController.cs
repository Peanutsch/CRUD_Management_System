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
        return View(); // Model met gegenereerde alias naar de view sturen
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

        // Maak een nieuwe UserLoginModel instantie voor de Users tabel
        var newUserLogin = new UserLoginModel
        {
            AliasId = newUser.Alias,  // Gebruik het alias als AliasId
            Password = newUser.Alias,  // Zet het alias als wachtwoord
            Admin = false,  // Zet Admin op false
            OnlineStatus = false,  // Zet OnlineStatus op false
            TheOne = false  // Zet TheOne op false
        };

        // Voeg de nieuwe gebruiker toe aan zowel UserDetails als Users tabel
        _context.Users.Add(newUserLogin); // Voeg toe aan de Users tabel
        _context.UserDetails.Add(newUser); // Voeg toe aan de UserDetails tabel
        await _context.SaveChangesAsync();

        // Redirect naar dashboard
        return RedirectToAction("Index", "DashboardAdmin");
    }

    [HttpPost]
    public async Task<IActionResult> GenerateAlias(string name, string surname)
    {
        Debug.WriteLine($"[CreateUser.GenerateAlias(name, surname)] Received name: {name}, surname: {surname}");

        var alias = await _aliasService.CreateTXTAlias(name, surname);

        Debug.WriteLine($"Generated alias: {alias}");

        return Json(alias);
    }
}
