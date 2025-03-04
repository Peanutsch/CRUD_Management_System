using CRUD_Management_System.Data;
using CRUD_Management_System.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Service for generating and managing user aliases.
/// </summary>
public class AliasService
{
    private readonly AppDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="AliasService"/> class.
    /// </summary>
    /// <param name="context">The database context for checking alias existence.</param>
    public AliasService(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Generates a unique alias for a user based on the first two letters of their first name
    /// and the last two letters of their surname, followed by a number that increments if the alias already exists.
    /// Diacritical marks in letters like é, è, and ô are removed for the alias.
    /// </summary>
    /// <param name="Name">The first name of the user.</param>
    /// <param name="Surname">The surname of the user.</param>
    /// <returns>A unique alias as a string.</returns>
    public async Task<string> CreateTXTAlias(string Name, string Surname)
    {
        // Normalize and remove diacritics from the Name and Surname
        Name = RemoveDiacritics(Name);
        Surname = RemoveDiacritics(Surname);

        // Ensure Name and Surname have at least two characters
        if (Name.Length < 2)
        {
            Name += Name; // Duplicate the name if too short
        }
        if (Surname.Length < 2)
        {
            Surname += Surname; // Duplicate the surname if too short
        }

        // Construct the initial alias (first two letters of Name + last two letters of Surname)
        string initialAlias = Name.Substring(0, 2).ToLower() + Surname.Substring(Surname.Length - 2).ToLower();
        int counter = 1;
        string finalAlias = initialAlias + "001"; // Start with "001" to represent the first alias

        // Check if the alias already exists and increment the number if necessary
        while (await AliasExistsAsync(finalAlias)) // Use await to call the async method
        {
            counter++;
            string newNumber = counter.ToString("D3"); // Ensures it always has 3 digits
            finalAlias = initialAlias + newNumber;
        }

        return finalAlias;
    }

    /// <summary>
    /// Removes diacritical marks from a given string.
    /// </summary>
    /// <param name="text">The input string containing diacritics.</param>
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
    /// Checks if the given alias already exists in the database.
    /// </summary>
    /// <param name="alias">The alias to check for existence.</param>
    /// <returns>True if the alias exists; otherwise, false.</returns>
    public async Task<bool> AliasExistsAsync(string alias)
    {
        return await _context.UserDetails.AnyAsync(u => u.Alias == alias);
    }
}
