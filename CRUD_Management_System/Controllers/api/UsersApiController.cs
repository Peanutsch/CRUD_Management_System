using CRUD_Management_System.Data;
using CRUD_Management_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace CRUD_Management_System.Controllers.api
{
    // Define the route for the API controller. It will listen to requests starting with "/api/Users"
    [Route("api/users")]
    public class UsersApiController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsersApiController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDetailsModel>>> GetUsers()
        {
            Debug.WriteLine("API WERKT!");

            var users = await _context.UserDetails
                                       .Select(u => new UserDetailsModel
                                       {
                                           Name = u.Name,
                                           Surname = u.Surname,
                                           Alias = u.Alias,
                                           Address = u.Address,
                                           ZIP = u.ZIP,
                                           City = u.City,
                                           Email = u.Email,
                                           Phonenumber = u.Phonenumber,
                                           Online = u.Online,
                                           Sick = u.Sick
                                       })
                                       .ToListAsync();
            
            foreach (var user in users)
            {
                Debug.WriteLine($"User found: {user.Alias}, {user.Name}, {user.Surname}");
            }

            return Ok(users);
        }
    }
}
