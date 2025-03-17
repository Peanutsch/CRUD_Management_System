using CRUD_Management_System.Data;
using CRUD_Management_System.Models;
using CRUD_Management_System.Encryption;
using CRUD_Management_System.Services; // Don't forget to add the appropriate namespace for the service
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Threading.Tasks;

public class CreateUserController : Controller
{
    private readonly AppDbContext _context;
    private readonly AliasService _aliasService;

    /// <summary>
    /// Initializes the controller with the necessary services for database access and alias generation.
    /// </summary>
    /// <param name="context">Database context for accessing the user data.</param>
    /// <param name="aliasService">Service for generating user aliases.</param>
    public CreateUserController(AppDbContext context, AliasService aliasService)
    {
        _context = context;
        _aliasService = aliasService;
    }

    /// <summary>
    /// Displays the form for creating a new user.
    /// </summary>
    /// <returns>The view for creating a new user.</returns>
    public IActionResult Index()
    {
        return View(); // Display the form for the new user creation
    }

    /// <summary>
    /// Processes the form and saves the new user to the database.
    /// Generates a unique alias for the user and stores both user details and login information.
    /// </summary>
    /// <param name="newUser">The new user details to be saved.</param>
    /// <returns>A redirect to the dashboard if successful, or the same form with errors if validation fails.</returns>
    [HttpPost]
    public async Task<IActionResult> CreateUser(UserDetailsModel newUser)
    {
        // Generate the alias for the new user using the alias service
        newUser.Alias = await _aliasService.CreateTXTAlias(newUser.Name, newUser.Surname);

        if (!ModelState.IsValid)
        {
            // Log errors if there are validation issues
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Debug.WriteLine(error.ErrorMessage);
            }

            // Display an error message and return to the form with validation issues
            TempData["ErrorMessage"] = "An error occurred while creating the user.";
            return View("Index", newUser); // Redisplay the form with errors
        }

        // Generate a password for the user
        var generatedPassword = PasswordManager.PasswordGenerator();

        // Hash the password using BCrypt
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(generatedPassword);

        // Create a new UserLoginModel for the Users table
        var newUserLogin = new UserLoginModel
        {
            AliasId = newUser.Alias,  // Use the generated alias as the AliasId
            Password = hashedPassword,
            Admin = false,  // Set the Admin flag to false
            OnlineStatus = false,  // Set the OnlineStatus flag to false
            TheOne = false  // Set TheOne flag to false
        };

        Debug.WriteLine("\n[ NEW USER ACCOUNT ]");
        Debug.WriteLine($"AliasId: {newUserLogin.AliasId}");
        Debug.WriteLine($"Password: {generatedPassword}");
        Debug.WriteLine($"Admin: {newUserLogin.Admin}");
        Debug.WriteLine($"OnlineStatus: {newUserLogin.OnlineStatus}");
        Debug.WriteLine($"TheOne: {newUserLogin.TheOne}\n");


        // Add the new user to both the UserDetails and Users tables in the database
        _context.Users.Add(newUserLogin); // Add to the Users table (login info)
        _context.UserDetails.Add(newUser); // Add to the UserDetails table (personal info)
        await _context.SaveChangesAsync(); // Save the changes to the database

        // Redirect to the Admin Dashboard after successful creation
        return RedirectToAction("Index", "DashboardAdmin");
    }

    /// <summary>
    /// Generates a unique alias based on the provided name and surname.
    /// </summary>
    /// <param name="name">The first name of the user.</param>
    /// <param name="surname">The surname of the user.</param>
    /// <returns>A JSON response with the generated alias.</returns>
    [HttpPost]
    public async Task<IActionResult> GenerateAlias(string name, string surname)
    {
        // Generate a unique alias using the AliasService
        var alias = await _aliasService.CreateTXTAlias(name, surname);

        // Return the alias as a JSON response
        return Json(alias);
    }
}
