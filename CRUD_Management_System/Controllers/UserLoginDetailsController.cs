using CRUD_Management_System.Data;
using CRUD_Management_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRUD_Management_System.Controllers
{
    /// <summary>
    /// Controller for managing user login details.
    /// </summary>
    public class UserLoginDetailsController : Controller
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserLoginDetailsController"/> class.
        /// </summary>
        /// <param name="context">The database context for accessing user login details.</param>
        public UserLoginDetailsController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves a list of all users and passes it to the view.
        /// </summary>
        /// <returns>A view displaying the list of users.</returns>
        public async Task<IActionResult> Index()
        {
            // Retrieve the list of users from the database
            var users = await _context.Users.ToListAsync();

            // Pass the list of users to the view
            return View(users);
        }

        /// <summary>
        /// Displays the details of a specific user based on their alias ID.
        /// </summary>
        /// <param name="aliasId">The alias ID of the user.</param>
        /// <returns>A view displaying the user's details, or a NotFound result if the user does not exist.</returns>
        public async Task<IActionResult> Details(string aliasId)
        {
            if (aliasId == null)
            {
                return NotFound();
            }

            // Retrieve the user from the database based on the alias ID
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
