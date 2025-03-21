using CRUD_Management_System.Data;
using CRUD_Management_System.Models;
using CRUD_Management_System.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

public class EditUserDetailsController : Controller
{
    private readonly AppDbContext _context;
    private readonly LogService _logService;

    /// <summary>
    /// Initializes a new instance of the <see cref="EditUserDetailsController"/> class.
    /// </summary>
    /// <param name="context">Database context for accessing user data.</param>
    public EditUserDetailsController(AppDbContext context, LogService logService)
    {
        _context = context;
        _logService = logService;
    }

    /// <summary>
    /// Retrieves user details for editing based on the provided alias.
    /// </summary>
    /// <param name="alias">The alias of the user to be edited.</param>
    /// <returns>The user details view if found, otherwise a NotFound result.</returns>
    public async Task<IActionResult> Index(string alias)
    {
        if (string.IsNullOrEmpty(alias))
        {
            return NotFound("No alias provided.");
        }

        var user = await _context.UserDetails.FirstOrDefaultAsync(u => u.Alias == alias);
        if (user == null)
        {
            return NotFound("User not found.");
        }

        return View(user);
    }

    /// <summary>
    /// Saves the updated user details to the database.
    /// </summary>
    /// <param name="updatedUser">The updated user details model.</param>
    /// <returns>Redirects to the admin dashboard if successful, otherwise returns the edit view.</returns>
    [HttpPost]
    public async Task<IActionResult> SaveEdit(UserDetailsModel updatedUser)
    {
        if (!ModelState.IsValid)
        {
            return View("Index", updatedUser);
        }

        var user = await _context.UserDetails.FirstOrDefaultAsync(u => u.Alias == updatedUser.Alias);
        if (user == null)
        {
            return NotFound("User not found.");
        }

        // Update user details
        user.Name = updatedUser.Name;
        user.Surname = updatedUser.Surname;
        user.Address = updatedUser.Address;
        user.ZIP = updatedUser.ZIP;
        user.City = updatedUser.City;
        user.Email = updatedUser.Email;
        user.Phonenumber = updatedUser.Phonenumber;
        user.Online = updatedUser.Online;
        user.Sick = updatedUser.Sick;

        await _context.SaveChangesAsync();

        var token = Request.Cookies["AuthToken"];
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

        var currentUser = jsonToken!.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
        _logService.LogEditUserDetails(updatedUser, currentUser!);

        return RedirectToAction("Index", "DashboardAdmin");
    }
}
