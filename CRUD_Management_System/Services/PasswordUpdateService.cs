using BCrypt.Net;
using System.Linq;
using CRUD_Management_System.Data;
using CRUD_Management_System.Models;

namespace CRUD_Management_System.Services
{
    public class PasswordUpdateService
    {
        private readonly AppDbContext _dbContext;

        public PasswordUpdateService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void UpdatePasswordsToHash()
        {
            // Haal alle gebruikers op uit de database die geen gehashte wachtwoorden hebben
            var users = _dbContext.Users.Where(u => !string.IsNullOrEmpty(u.Password)).ToList();

            foreach (var user in users)
            {
                // Controleer of het wachtwoord al een geldige BCrypt hash is
                if (!user.Password.StartsWith("$2a$"))
                {
                    // Encrypt het wachtwoord met BCrypt en sla het op
                    string hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password);
                    user.Password = hashedPassword;
                }
            }

            // Sla de bijgewerkte gebruikers op in de database
            _dbContext.SaveChanges();
        }
    }
}
