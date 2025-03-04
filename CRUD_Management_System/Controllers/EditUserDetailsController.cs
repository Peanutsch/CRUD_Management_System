using CRUD_Management_System.Data;
using CRUD_Management_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

public class EditUserDetailsController : Controller
{
    private readonly AppDbContext _context;

    public EditUserDetailsController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(string alias)
    {
        if (string.IsNullOrEmpty(alias))
        {
            return NotFound("Geen alias opgegeven.");
        }

        var user = await _context.UserDetails.FirstOrDefaultAsync(u => u.Alias == alias);
        if (user == null)
        {
            return NotFound("Gebruiker niet gevonden.");
        }

        return View(user);
    }

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
            return NotFound("Gebruiker niet gevonden.");
        }

        // Update de gegevens
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
        return RedirectToAction("Index", "DashboardAdmin");
    }
}
