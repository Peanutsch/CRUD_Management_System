using CRUD_Management_System.Data;
using CRUD_Management_System.Models;
using CRUD_Management_System.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

public class UserController : Controller
{
    private readonly AppDbContext _context;
    private readonly LogService _logService;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserController"/> class.
    /// </summary>
    /// <param name="context">The database context for accessing user data.</param>
    public UserController(AppDbContext context, LogService logService)
    {
        _context = context;
        _logService = logService;
    }

    /// <summary>
    /// Updates an existing user's details.
    /// </summary>
    /// <param name="update">The user details to be updated.</param>
    /// <returns>An HTTP response indicating success or failure.</returns>
    [HttpPost]
    public async Task<IActionResult> UpdateUser([FromBody] UserDetailsModel update)
    {
        // Validate input
        if (update == null || string.IsNullOrEmpty(update.Alias))
        {
            return BadRequest("Invalid user data.");
        }

        // Find the user by alias
        var user = await _context.UserDetails.FirstOrDefaultAsync(u => u.Alias == update.Alias);
        if (user == null)
        {
            return NotFound("User not found.");
        }

        // Update user fields
        user.Name = update.Name;
        user.Surname = update.Surname;
        user.Address = update.Address;
        user.ZIP = update.ZIP;
        user.City = update.City;
        user.Email = update.Email;
        user.Phonenumber = update.Phonenumber;
        user.Online = update.Online;
        user.Sick = update.Sick;

        // Save changes to the database
        await _context.SaveChangesAsync();

        return Ok(user); // Return the updated user as JSON
    }

    /// <summary>
    /// Deletes a user from the system.
    /// </summary>
    /// <param name="request">The alias of the user to be deleted.</param>
    /// <returns>An HTTP response indicating success or failure.</returns>
    [HttpDelete]
    public async Task<IActionResult> DeleteUser([FromBody] UserDeleteRequest request)
    {
        // Validate input
        if (request == null || string.IsNullOrEmpty(request.Alias))
        {
            return BadRequest(new { message = "Invalid user alias." });
        }

        // Find the user and their details by alias
        var user = await _context.Users.FirstOrDefaultAsync(u => u.AliasId == request.Alias);
        var userDetails = await _context.UserDetails.FirstOrDefaultAsync(u => u.Alias == request.Alias);

        if (user == null || userDetails == null)
        {
            return NotFound(new { success = false, message = "User not found." });
        }

        // Remove user and their details from the database
        _context.Users.Remove(user);
        _context.UserDetails.Remove(userDetails);
        await _context.SaveChangesAsync();

        var token = Request.Cookies["AuthToken"];
        var (currentUser, userRole) = LoginController.GetUserFromToken(token!);

        _logService.LogDeleteUserAccount(currentUser!, user.AliasId);

        return Ok(new { success = true, message = "User successfully deleted." });
    }
}
