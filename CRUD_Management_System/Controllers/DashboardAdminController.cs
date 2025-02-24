using Microsoft.AspNetCore.Mvc;
using CRUD_Management_System.Models;
using Microsoft.EntityFrameworkCore;
using CRUD_Management_System.Data;

namespace CRUD_Management_System.Controllers
{
    public class DashboardAdminController : Controller
    {
        private readonly AppDbContext _context;

        public DashboardAdminController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult>Index()
        {
            var users = await _context.UserDetails.ToListAsync();
            ViewData["CurrentUser"] = TempData["CurrentUser"]; // Haal de waarde uit TempData
            return View(users);
        }
    }
}
