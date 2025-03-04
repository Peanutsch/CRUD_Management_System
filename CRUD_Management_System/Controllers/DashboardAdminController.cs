using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CRUD_Management_System.Data;

namespace CRUD_Management_System.Controllers
{
    public class DashboardAdminController : Controller
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="DashboardAdminController"/> class.
        /// </summary>
        /// <param name="context">Database context for accessing user data.</param>
        public DashboardAdminController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves a list of users and displays them in the dashboard view.
        /// Retrieves the current user from TempData for display purposes.
        /// </summary>
        /// <returns>A view displaying the list of users and the current user.</returns>
        public async Task<IActionResult> Index()
        {
            // Retrieve the list of users from the UserDetails table
            var users = await _context.UserDetails.ToListAsync();

            // Retrieve the current user from TempData, which was set during login
            ViewData["CurrentUser"] = TempData["CurrentUser"];

            // Return the view with the list of users
            return View(users);
        }
    }
}
