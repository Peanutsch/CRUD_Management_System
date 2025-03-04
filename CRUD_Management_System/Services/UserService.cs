using CRUD_Management_System.Models;
using CRUD_Management_System.Data;
using CRUD_Management_System.Services;

namespace CRUD_Management_System.Services
{
    /// <summary>
    /// Service for managing user-related operations such as creating and deleting users.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly AliasService _aliasService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserService"/> class.
        /// </summary>
        /// <param name="context">Database context for accessing user data.</param>
        /// <param name="aliasService">Service for generating user aliases.</param>
        public UserService(AppDbContext context, AliasService aliasService)
        {
            _context = context;
            _aliasService = aliasService;
        }

        /// <summary>
        /// Deletes a user from both the Users and UserDetails tables based on the alias.
        /// </summary>
        /// <param name="alias">The alias of the user to be deleted.</param>
        /// <returns>True if the user was successfully deleted; otherwise, false.</returns>
        public bool DeleteUserByAlias(string alias)
        {
            var users = _context.Users.SingleOrDefault(u => u.AliasId == alias);
            var userDetails = _context.UserDetails.SingleOrDefault(u => u.Alias == alias);

            if (users == null || userDetails == null)
            {
                return false; // User does not exist, return false.
            }

            // Remove user records from both tables
            _context.Users.Remove(users);
            _context.UserDetails.Remove(userDetails);

            _context.SaveChanges(); // Commit changes to the database
            return true;
        }

        /// <summary>
        /// Creates a new user by generating an alias and saving user details in the database.
        /// </summary>
        /// <param name="newUser">The user details to be created.</param>
        /// <returns>True if the user was successfully created; otherwise, false.</returns>
        public async Task<bool> CreateUser(UserDetailsModel newUser)
        {
            if (newUser == null)
            {
                return false; // Invalid input, return false.
            }

            // Generate an alias for the new user using the alias service.
            newUser.Alias = await _aliasService.CreateTXTAlias(newUser.Name, newUser.Surname);

            // Create a new UserLoginModel instance for the Users table.
            var newUserLogin = new UserLoginModel
            {
                AliasId = newUser.Alias,  // Set alias as AliasId
                Password = newUser.Alias, // Use alias as the password (should be hashed in production)
                Admin = false,            // Default: Not an admin
                OnlineStatus = false,     // Default: Not online
                TheOne = false            // Default: Not a special admin
            };

            // Add the new user to both UserDetails and Users tables.
            _context.Users.Add(newUserLogin);
            _context.UserDetails.Add(newUser);

            await _context.SaveChangesAsync(); // Save changes asynchronously
            return true;
        }
    }
}
