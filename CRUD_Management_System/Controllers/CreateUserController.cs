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
using Microsoft.AspNetCore.Authorization;

public class CreateUserController : Controller
{
    private readonly AppDbContext _context;
    private readonly AliasService _aliasService;
    private readonly LogService _logService;

    public CreateUserController(AppDbContext context, AliasService aliasService, LogService logService)
    {
        _context = context;
        _aliasService = aliasService;
        _logService = logService;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateUser([FromBody] UserDetailsModel newUser)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            return BadRequest(new { success = false, message = "Validation failed", errors });
        }

        Debug.WriteLine($"[DEBUG CreateUser] Name: {newUser.Name}, Surname: {newUser.Surname}, Alias: {newUser.Alias}");

        // Alias genereren als deze niet is meegegeven door de frontend
        if (string.IsNullOrEmpty(newUser.Alias))
        {
            Debug.WriteLine("[DEBUG CreateUser] Alias is empty, generating one...");
            newUser.Alias = await _aliasService.CreateTXTAlias(newUser.Name, newUser.Surname);
        }

        // Genereer en hash het wachtwoord
        var generatedPassword = PasswordManager.PasswordGenerator();
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(generatedPassword);

        var newUserLogin = CreateUserLogin(newUser, hashedPassword);

        // Log user creationS;
        _logService.LogNewUserDetails(newUserLogin, generatedPassword, User.Identity?.Name ?? "Unknown");

        await AddUserToDatabase(newUser, newUserLogin);

        return Ok(new { success = true, message = "User created successfully." });
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
