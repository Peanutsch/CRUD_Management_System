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

    #region [OLD INDEX]
    /*
    /// <summary>
    /// Handles the login process when the user submits their credentials.
    /// </summary>
    /// <param name="model">The login model containing user credentials.</param>
    /// <returns>Redirects to DashboardAdmin on success, otherwise reloads the login view with an error message.</returns>
    [HttpPost]
    //[ValidateAntiForgeryToken]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> Index(LoginModel model)
    {
        if (ModelState.IsValid)
        {
            // Zoek de gebruiker op basis van AliasId
            var user = await _context.Users.FirstOrDefaultAsync(u => u.AliasId == model.AliasId);

            Console.WriteLine($"Index > Ingevoerd wachtwoord: {model.Password}");
            Console.WriteLine($"Index > Gehasht wachtwoord in DB: {user?.Password}");

            // Als de gebruiker bestaat en het wachtwoord correct is
            if (user != null && BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
            {
                // Maak een JWT-token aan voor de gebruiker
                var token = GenerateJwtToken(user);

                // Sla de gebruiker en de rol op in TempData
                TempData["CurrentUser"] = user.AliasId;  // Bewaar de AliasId in TempData
                TempData["Role"] = user.Admin.ToString();  // Bewaar de rol in TempData

                // Stuur de JWT-token naar de frontend
                return Ok(new { Token = token });  // Dit stuurt de JWT token als onderdeel van de response
            }

            // Voeg een foutmelding toe als de login niet succesvol is
            ModelState.AddModelError("", "Invalid login credentials");
        }

        // Als model validatie of login niet succesvol is, herlaad het loginform
        return BadRequest(new { message = "Invalid login credentials" });

        //return View(model);
    }
    */
    #endregion [OLD INDEX]

    // POST method to handle user login
    [HttpPost]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        Debug.WriteLine("Login method reached");

        Debug.WriteLine($"Login > Ontvangen AliasId: {model.AliasId}");
        Debug.WriteLine($"Login > Ontvangen Password: {model.Password}");


        if (ModelState.IsValid)  // Check if the model is valid (i.e., required fields are filled)
        {
            // Retrieve the user from the database based on the alias provided in the login model
            var user = await _context.Users.FirstOrDefaultAsync(u => u.AliasId == model.AliasId);

            Debug.WriteLine($"Login > Ingevoerd wachtwoord: {model.Password}");
            Debug.WriteLine($"Login > Gehasht wachtwoord in DB: {user?.Password}");

            // If user is found and password verification is successful
            if (user != null && BCrypt.Net.BCrypt.Verify(model.Password, user.Password))  // Verifying password with BCrypt
            {
                // Create a JWT token for the authenticated user
                var token = GenerateJwtToken(user);

                // Send the token as a response, which the frontend can use for subsequent requests
                return Ok(new { Token = token });  // HTTP 200 status with the token
            }

            // If the user is not found or password is incorrect, return an error message
            ModelState.AddModelError("", "Invalid login credentials");
        }

        // If model validation fails or login credentials are invalid, return to the login view
        return BadRequest(new { message = "Invalid login credentials" });

        //return View(model);
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

