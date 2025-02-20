using CRUD_Management_System.Data;
using CRUD_Management_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRUD_Management_System.Controllers
{
    public class UserLoginDetailsController : Controller
    {
        private readonly AppDbContext _context;

        public UserLoginDetailsController(AppDbContext context)
        {
            _context = context;
        }

        // Actie om gebruikers op te halen en naar de view door te sturen
        public async Task<IActionResult> Index()
        {
            // Haal de lijst van gebruikers op uit de database
            var users = await _context.Users.ToListAsync();

            // Stuur de lijst van gebruikers naar de view
            return View(users);
        }

        // Een actie voor het bekijken van details van een specifieke gebruiker
        public async Task<IActionResult> Details(string aliasId)
        {
            if (aliasId == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.AliasId == aliasId);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }
    }
}
