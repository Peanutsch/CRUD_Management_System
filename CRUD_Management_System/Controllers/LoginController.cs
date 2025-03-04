using Microsoft.AspNetCore.Mvc;
using CRUD_Management_System.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CRUD_Management_System.Data;

public class LoginController : Controller
{
    private readonly AppDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="LoginController"/> class.
    /// </summary>
    /// <param name="context">The database context for accessing user data.</param>
    public LoginController(AppDbContext context)
    {
        _context = context;
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

        // Check if a user exists with the given alias and password
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.AliasId == model.AliasId && u.Password == model.Password);

        if (user != null)
        {
            // Login successful, store the username in TempData and redirect to DashboardAdmin
            TempData["CurrentUser"] = user.AliasId;  // TempData is used to temporarily store the logged-in user
            return RedirectToAction("Index", "DashboardAdmin");
        }

        // Login failed, add an error message and reload the login view
        ModelState.AddModelError("", "Invalid login credentials");
        return View(model);
    }
}
