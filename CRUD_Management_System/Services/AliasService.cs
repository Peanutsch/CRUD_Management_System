using CRUD_Management_System.Data;
using CRUD_Management_System.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;

public class AliasService
{
    private readonly AppDbContext _context;

    public AliasService(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Generates a unique alias for the user based on the first two letters of the first name
    /// and the last two letters of the surname, followed by a number that increments if the alias already exists.
    /// Diacritics in letters like é, è, and ô are removed for the alias.
    /// </summary>
    /// <returns>A unique alias as a string.</returns>
    public async Task<string> CreateTXTAlias(string Name, string Surname)
    {
        // Normalize and remove diacritics from the Name and Surname
        Name = RemoveDiacritics(Name);
        Surname = RemoveDiacritics(Surname);

        if (Name.Length < 2)
        {
            Name += Name; // Double name if it is too short
        }
        if (Surname.Length < 2)
        {
            Surname += Surname; // Double surname if it is too short
        }

        string initialAlias = Name.Substring(0, 2).ToLower() + Surname.Substring(Surname.Length - 2).ToLower();
        int counter = 1;
        string finalAlias = initialAlias + "001"; // Start with "001" to represent the first alias

        // Check if the alias already exists and increment the number if necessary
        while (await AliasExistsAsync(finalAlias)) // Use await here to call the async method
        {
            counter++;
            string newNumber = counter.ToString("D3"); // Ensures it always has 3 digits
            finalAlias = initialAlias + newNumber;
        }

        return finalAlias;
    }

    /// <summary>
    /// Removes diacritics from the input string.
    /// </summary>
    /// <param name="text">The input string with potential diacritics.</param>
    /// <returns>A string without diacritics.</returns>
    private string RemoveDiacritics(string text)
    {
        var normalizedString = text.Normalize(NormalizationForm.FormD);
        var stringBuilder = new StringBuilder();

        foreach (var chars in normalizedString)
        {
            var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(chars);
            if (unicodeCategory != UnicodeCategory.NonSpacingMark)
            {
                stringBuilder.Append(chars);
            }
        }

        return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
    }

    /// <summary>
    /// Checks if the given alias already exists in the database file.
    /// </summary>
    /// <param name="alias">The alias to check for existence.</param>
    /// <returns>True if the alias exists; otherwise, false.</returns>
    public async Task<bool> AliasExistsAsync(string alias)
    {
        // Controleer of de alias al bestaat in de database
        return await _context.UserDetails.AnyAsync(u => u.Alias == alias);
    }
}
