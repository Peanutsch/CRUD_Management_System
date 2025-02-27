using CRUD_Management_System.Models;
using CRUD_Management_System.Data;
using CRUD_Management_System.Services;

namespace CRUD_Management_System.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public bool DeleteUserByAlias(string alias)
        {
            var users = _context.Users.SingleOrDefault(u => u.AliasId == alias);
            var userDetails = _context.UserDetails.SingleOrDefault(u => u.Alias == alias);
            if (users == null || userDetails == null)
            {
                return false;
            }

            _context.Users.Remove(users);
            _context.UserDetails.Remove(userDetails);
            _context.SaveChanges();
            return true;
        }
    }
}
