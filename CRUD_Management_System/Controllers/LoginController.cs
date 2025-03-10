using Microsoft.AspNetCore.Mvc;
using CRUD_Management_System.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CRUD_Management_System.Data;
using System.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class LoginController : Controller
{
    private readonly IConfiguration _configuration;
    private readonly AppDbContext _context;

    // Constructor to inject configuration and database context
    public LoginController(IConfiguration configuration, AppDbContext context)
    {
        _configuration = configuration;  // Access application settings
        _context = context;  // Database context to interact with the Users table
    }

    /// <summary>
    /// Displays the login page.
    /// </summary>
    /// <returns>The login view.</returns>
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    /// <summary>
    /// Handles the login process when the user submits their credentials.
    /// </summary>
    /// <param name="model">The login model containing user credentials.</param>
    /// <returns>Redirects to DashboardAdmin on success, otherwise reloads the login view with an error message.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(LoginModel model)
    {
        // Validate the model
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        // Check if a user exists with the given alias
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.AliasId == model.AliasId);

        if (user != null && !string.IsNullOrEmpty(user.Password) && user.VerifyPassword(model.Password!))
        {
            // Login successful, store the username in TempData and redirect to DashboardAdmin
            TempData["CurrentUser"] = user.AliasId;  // TempData is used to temporarily store the logged-in user
            TempData["Role"] = user.Admin.ToString();
            return RedirectToAction("Index", "DashboardAdmin");
        }

        // Login failed, add an error message and reload the login view
        ModelState.AddModelError("", "Invalid login credentials");
        Debug.WriteLine("> Wrong Password...");

        return View(model);
    }

    // Private method to generate a JWT token for the user
    private string GenerateJwtToken(UserLoginModel user)
    {
        // Define claims (user's details) that will be included in the JWT token
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.AliasId),  // Subject claim, stores the user AliasId
            new Claim(ClaimTypes.Name, user.AliasId),  // Name claim, stores the user AliasId
            new Claim("role", user.Admin ? "Admin" : "User"),  // Role claim, assigns "Admin" or "User" based on the user's role
        };

        // Create a symmetric security key based on the secret key defined in the appsettings.json file
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);  // Signing credentials using HMAC-SHA256 algorithm

        // Create the JWT token with the specified claims, issuer, audience, and expiration time
        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],  // Issuer of the token (usually your server or service)
            audience: _configuration["Jwt:Audience"],  // Intended audience of the token
            claims: claims,  // Claims representing the user's details
            expires: DateTime.Now.AddDays(1),  // Set the token expiration to 1 day
            signingCredentials: creds  // Sign the token with the credentials
        );

        // Return the token as a string, which can be sent as part of the HTTP response
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

