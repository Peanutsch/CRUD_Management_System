using Microsoft.AspNetCore.Mvc;
using CRUD_Management_System.Models;
using Microsoft.EntityFrameworkCore;
using CRUD_Management_System.Data;
using X.PagedList;
using X.PagedList.Extensions;

namespace CRUD_Management_System.Controllers
{
    public class DashboardAdminController : Controller
    {
        private readonly AppDbContext _context;

        public DashboardAdminController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult>Index(int? page)
        {
            int pageSize = 6; // Maximaal aantal gebruikers per pagina
            int pageNumber = page ?? 1; // Standaard op pagina 1 starten

            var users = await _context.UserDetails.ToListAsync();
            var pagedUsers = users.ToPagedList(pageNumber, pageSize);

            ViewData["CurrentUser"] = TempData["CurrentUser"]; // Haal de waarde uit TempData
            return View(pagedUsers);
        }
    }
}
