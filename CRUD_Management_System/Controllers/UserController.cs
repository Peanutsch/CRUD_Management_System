using CRUD_Management_System.Data;
using CRUD_Management_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

public class UserController : Controller
{
    private readonly AppDbContext _context;

    public UserController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> UpdateUser([FromBody] UserDetailsModel userData)
    {
        if (userData == null || string.IsNullOrEmpty(userData.Alias))
        {
            return BadRequest("Ongeldige gebruikersgegevens.");
        }

        var user = await _context.UserDetails.FirstOrDefaultAsync(u => u.Alias == userData.Alias);
        if (user == null)
        {
            return NotFound("Gebruiker niet gevonden.");
        }

        // Update velden
        user.Name = userData.Name;
        user.Surname = userData.Surname;
        user.Address = userData.Address;
        user.ZIP = userData.ZIP;
        user.City = userData.City;
        user.Email = userData.Email;
        user.Phonenumber = userData.Phonenumber;
        user.Online = userData.Online;
        user.Sick = userData.Sick;

        // Opslaan in de database
        await _context.SaveChangesAsync();

        return Ok(user); // Stuur de bijgewerkte gebruiker terug als JSON
    }
}
