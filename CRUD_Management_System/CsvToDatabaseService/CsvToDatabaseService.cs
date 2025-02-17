using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CRUD_Management_System.Data;
using CRUD_Management_System.Models;
using Microsoft.EntityFrameworkCore;

public class CsvToDatabaseService
{
    private readonly AppDbContext _context;

    public CsvToDatabaseService(AppDbContext context)
    {
        _context = context;
    }

    public async Task ImportUserLoginsFromCsvAsync(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException("CSV-bestand niet gevonden", filePath);
        }

        var lines = await File.ReadAllLinesAsync(filePath);

        foreach (var line in lines.Skip(1)) // Skip header row
        {
            var fields = line.Split(',');

            if (fields.Length < 3)
                continue; // Asure there are enough fields

            // Convert string to booleans
            bool.TryParse(fields[2].Trim(), out bool admin);
            bool.TryParse(fields[3].Trim(), out bool onlineStatus);
            bool.TryParse(fields[4].Trim(), out bool theOne);

            var user = new UserLoginModel
            {
                AliasId = fields[0].Trim(),
                Password = fields[1].Trim(),
                Admin = admin,
                OnlineStatus = onlineStatus,
                TheOne = theOne
            };

            if (!await _context.Users.AnyAsync(u => u.AliasId == user.AliasId))
            {
                await _context.Users.AddAsync(user);
            }
        }

        await _context.SaveChangesAsync();
    }
}
