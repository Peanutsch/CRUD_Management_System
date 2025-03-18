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
using System.Data;
using Newtonsoft.Json;
using CRUD_Management_System.Services;
using Serilog;

public class LoginController : Controller
{
    private readonly IConfiguration _configuration;
    private readonly AppDbContext _context;
    private readonly LogService _logService;

    // Constructor to inject configuration and database context
    public LoginController(IConfiguration configuration, AppDbContext context, LogService logService)
    {
        _configuration = configuration;  // Access application settings (e.g., JWT secret, issuer, etc.)
        _context = context;  // Database context to interact with the Users table in the database
        _logService = logService;
    }

    /// <summary>
    /// Displays the login page view.
    /// </summary>
    /// <returns>The login view (Index view).</returns>
    [HttpGet]
    public IActionResult Index()
    {
        return View();  // Returns the login page view
    }

    /// <summary>
    /// Handles the login process by verifying user credentials and generating a JWT token.
    /// </summary>
    /// <param name="model">The login model containing the alias (username) and password provided by the user.</param>
    /// <returns>Redirects to a view or returns a JSON response with a token.</returns>
    [HttpPost]
    //[IgnoreAntiforgeryToken]  // Disables CSRF protection for this action
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        if (ModelState.IsValid)  // Check if the model passed from the frontend is valid
        {
            // Retrieve user from the database based on AliasId (username)
            var user = await _context.Users.FirstOrDefaultAsync(u => u.AliasId == model.AliasId);

            // If the user exists and the password matches
            if (user != null && BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
            {
                // Generate a JWT token for the authenticated user
                var token = GenerateJwtToken(user);

                // Store the token in a secure cookie (HttpOnly, Secure, SameSite)
                Response.Cookies.Append("AuthToken", token, new CookieOptions
                {
                    // HttpOnly is temporarily off here for JavaScript access to the token, usually should be enabled

                    // HttpOnly = true,                     // Protects against XSS (Cross-Site Scripting) attacks
                    Secure = true,                          // Ensures the cookie is only sent over HTTPS
                    Expires = DateTime.UtcNow.AddDays(1),   // Set the cookie expiration to 1 day
                    SameSite = SameSiteMode.Strict,         // Restrict the cookie to same-site requests only
                    Path = "/"                              // Cookie is accessible for all pages in the application
                });
                
                var (currentUser, userRole) = GetUserFromToken(token); // Get user and role
                _logService.LogLogin(currentUser.ToUpper());           // Log the login with both username and role

                return Ok(new { token });  // Return the token as part of the response to the client
            }
        }

        return View(model);  // If the login fails, return the login view with the model
    }

    private static (string currentUser, string userRole) GetUserFromToken(string token)
    {
        var jsonToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
        var userRole = jsonToken.Claims.FirstOrDefault(c => c.Type == "role")?.Value ?? "Unknown";
        var currentUser = jsonToken.Claims.FirstOrDefault(c => c.Type == "sub")?.Value ?? "Unknown";
        return (currentUser, userRole);
    }

    /// <summary>
    /// Generates a JWT (JSON Web Token) for the authenticated user.
    /// </summary>
    /// <param name="user">The authenticated user whose information will be included in the token.</param>
    /// <returns>A JWT token string.</returns>
    private string GenerateJwtToken(UserLoginModel user)
    {
        // Define the claims (user-specific information) to be included in the JWT token
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.AliasId),  // AliasId (username) as the subject of the token
            new Claim(ClaimTypes.Name, user.AliasId),  // Name claim: AliasId (username)
            new Claim("role", user.Admin ? "admin" : "user"),  // Role claim: "admin" if the user is an admin, otherwise "user"
        };

        // Create a symmetric security key based on the secret key from the appsettings.json
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);  // Signing credentials with HMAC-SHA256

        // Create the JWT token with claims, expiration date, and signing credentials
        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],  // Issuer of the token (usually the API server)
            audience: _configuration["Jwt:Audience"],  // Intended audience for the token (client apps)
            claims: claims,  // Include the user-specific claims (AliasId, role)
            expires: DateTime.Now.AddDays(1),  // Token expires in 1 day
            signingCredentials: creds  // Sign the token using the credentials
        );

        // Convert the token to a string and return it
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
