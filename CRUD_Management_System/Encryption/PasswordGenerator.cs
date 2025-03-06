using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;

namespace CRUD_Management_System.Encryption;

/// <summary>
/// Provides methods for securely generating and verifying password hashes using 
/// the PBKDF2 (Password-Based Key Derivation Function 2) algorithm with SHA-256.
/// Also includes methods for encrypting and decrypting CSV files.
/// </summary>
internal class PasswordManager
{
    #region GENERATOR
    /// <summary>
    /// Generates a secure random password of the specified length.
    /// </summary>
    /// <param name="length">The length of the password to be generated.</param>
    /// <returns>A random password string.</returns>
    public static string PasswordGenerator(int length = 12)
    {
        const string lowerCaseChars = "abcdefghijklmnopqrstuvwxyz";
        const string upperCaseChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        const string numericChars = "1234567890";

        // Ensure minimum length of 12 characters
        if (length < 12)
            length = 12;

        // Create lists for each character type
        List<char> password = new List<char>();
        byte[] randomBytes = new byte[length];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomBytes);
        }

        // Add at least 3 uppercase letters
        for (int index = 0; index < 3; index++)
        {
            password.Add(upperCaseChars[randomBytes[index] % upperCaseChars.Length]);
        }

        // Add at least 3 numbers
        for (int index = 3; index < 6; index++)
        {
            password.Add(numericChars[randomBytes[index] % numericChars.Length]);
        }

        // Fill the remaining characters with random lowercase or uppercase letters and numbers
        const string validChars = lowerCaseChars + upperCaseChars + numericChars;
        for (int index = 6; index < length; index++)
        {
            password.Add(validChars[randomBytes[index] % validChars.Length]);
        }

        // Shuffle to ensure randomness
        password = password.OrderBy(_ => randomBytes[new Random().Next(randomBytes.Length)]).ToList();

        return new string(password.ToArray());
    }
    #endregion GENERATOR
}