using CRUD_Management_System.Data;
using CRUD_Management_System.Models;
using CRUD_Management_System.Encryption;
using CRUD_Management_System.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Linq;
using Serilog;
using Mysqlx;

public class CreateUserController : Controller
{
    private readonly AppDbContext _context;
    private readonly AliasService _aliasService;
    private readonly LogNewUserService _logNewUserService;

    public CreateUserController(AppDbContext context, AliasService aliasService, LogNewUserService logNewUserService)
    {
        _context = context;
        _aliasService = aliasService;
        _logNewUserService = logNewUserService;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser(UserDetailsModel newUser)
    {
        // Velden leeg controleren
        CleanUserFields(newUser);

        // Alias genereren
        newUser.Alias = await _aliasService.CreateTXTAlias(newUser.Name, newUser.Surname);

        // Haal de huidige gebruiker op die de actie uitvoert
        var currentUser = User.Identity?.IsAuthenticated == true ? User.Identity.Name : "Unknown";

        if (!ModelState.IsValid)
        {
            // Haal het huidige foutbericht op
            var errors = string.Join("; ", ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage));

            // Log fouten bij mislukte validatie
            _logNewUserService.LogUserCreationError(errors, currentUser);

            TempData["ErrorMessage"] = "An error occurred while creating the user.";
            return View("Index", newUser);
        }

        // Genereer en hash het wachtwoord
        var generatedPassword = PasswordManager.PasswordGenerator();
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(generatedPassword);

        // Nieuwe user login record
        var newUserLogin = CreateUserLogin(newUser, hashedPassword);

        // Log de details voor debugging via de LogUserService
        _logNewUserService.LogUserDetails(newUserLogin, generatedPassword, currentUser);

        // Gebruiker toevoegen aan beide tabellen
        await AddUserToDatabase(newUser, newUserLogin);

        // Redirect naar de Admin Dashboard na succesvolle creatie
        return RedirectToAction("Index", "DashboardAdmin");
    }

    [HttpPost]
    public async Task<IActionResult> GenerateAlias(string name, string surname)
    {
        var alias = await _aliasService.CreateTXTAlias(name, surname);
        return Json(alias);
    }

    private void CleanUserFields(UserDetailsModel newUser)
    {
        newUser.Address = string.IsNullOrWhiteSpace(newUser.Address) ? string.Empty : newUser.Address;
        newUser.ZIP = string.IsNullOrWhiteSpace(newUser.ZIP) ? string.Empty : newUser.ZIP;
        newUser.City = string.IsNullOrWhiteSpace(newUser.City) ? string.Empty : newUser.City;
        newUser.Phonenumber = string.IsNullOrWhiteSpace(newUser.Phonenumber) ? string.Empty : newUser.Phonenumber;
    }

    private UserLoginModel CreateUserLogin(UserDetailsModel newUser, string hashedPassword)
    {
        return new UserLoginModel
        {
            AliasId = newUser.Alias,
            Password = hashedPassword,
            Admin = false,
            OnlineStatus = false,
            TheOne = false
        };
    }

    private async Task AddUserToDatabase(UserDetailsModel newUser, UserLoginModel newUserLogin)
    {
        _context.Users.Add(newUserLogin);
        _context.UserDetails.Add(newUser);
        await _context.SaveChangesAsync();
    }
}
