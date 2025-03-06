using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace CRUD_Management_System.Encryption;

internal static class EncryptionManager
{
    #region ENCRYPTION
    /// <summary>
    /// Encrypts the content of a CSV file and writes the encrypted content back to the same file.
    /// Each field in the CSV is encrypted individually using AES encryption.
    /// </summary>
    /// <param name="filePath">The path to the CSV file to be encrypted.</param>
    public static void EncryptFile(string filePath)
    {
        // Read all lines from the CSV file
        string[] lines = File.ReadAllLines(filePath);
        var encryptedLines = new List<string>();

        // Iterate through each line of the file
        foreach (string line in lines)
        {
            // Split the line into fields based on the comma separator
            string[] fields = line.Split(',');

            // Encrypt each field using a fixed encryption key
            for (int i = 0; i < fields.Length; i++)
            {
                fields[i] = AesEncryption.EncryptWithFixedKey(fields[i]);
            }

            // Combine the encrypted fields into a single line and add it to the encryptedLines list
            encryptedLines.Add(string.Join(",", fields));
        }

        // Write the encrypted lines back to the same file
        File.WriteAllLines(filePath, encryptedLines);
    }
    #endregion ENCRYPTION

    #region DECRYPTION
    /// <summary>
    /// Decrypts the content of an encrypted CSV file and writes the decrypted content back to the same file.
    /// Each field in the CSV is decrypted individually using AES decryption.
    /// </summary>
    /// <param name="filePath">The path to the CSV file to be decrypted.</param>
    public static void DecryptFile(string filePath)
    {
        // Read all lines from the encrypted CSV file
        string[] lines = File.ReadAllLines(filePath);
        var decryptedLines = new List<string>();

        // Iterate through each line of the file
        foreach (string line in lines)
        {
            // Split the line into fields based on the comma separator
            string[] fields = line.Split(',');

            // Decrypt each field using a fixed encryption key
            for (int i = 0; i < fields.Length; i++)
            {
                fields[i] = AesEncryption.DecryptWithFixedKey(fields[i], AesEncryption.GetKey());
            }

            // Combine the decrypted fields into a single line and add it to the decryptedLines list
            decryptedLines.Add(string.Join(",", fields));
        }

        // Write the decrypted lines back to the same file
        File.WriteAllLines(filePath, decryptedLines);
    }
    #endregion DECRYPTION
}