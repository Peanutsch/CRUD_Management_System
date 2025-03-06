using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace CRUD_Management_System.Encryption;
internal static class AesEncryption
{
    /// <summary>
    /// Retrieves encryption key (32 bytes) for AES encryption.
    /// </summary>
    /// <returns>Returns encryption key</returns>
    public static string GetKey()
    {
        //string keyPath = Path.Combine(RootPath.GetRootPath(), "Repositories", "key.txt");
        string keyPath = "";
        try
        {
            string getKey = File.ReadAllText(keyPath);
            return getKey;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error retrieving key: {ex}");
        }
        Debug.WriteLine("No key found...");
        return null!;
    }

    /// <summary>
    /// Encrypts the given plain text using a fixed encryption key and a random IV.
    /// The IV is prefixed to the encrypted data and returned as a Base64 string.
    /// </summary>
    /// <param name="plainText">The plain text string to be encrypted.</param>
    /// <returns>A Base64-encoded string containing the IV and encrypted data.</returns>
    public static string EncryptWithFixedKey(string plainText)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = GetKeyFromPassword(GetKey());
            aes.GenerateIV(); // Generate a random IV

            using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
            using (var ms = new MemoryStream())
            {
                // Write the IV at the beginning of the stream
                ms.Write(aes.IV, 0, aes.IV.Length);

                using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                using (var writer = new StreamWriter(cs))
                {
                    writer.Write(plainText);
                }

                // Convert the byte-array to a Base64-encoded string
                return Convert.ToBase64String(ms.ToArray());
            }
        }
    }

    /// <summary>
    /// Decrypts a Base64-encoded cipher text (containing IV and encrypted data) using a fixed password.
    /// </summary>
    /// <param name="cipherTextWithIvBase64">The Base64-encoded string containing the IV and encrypted data.</param>
    /// <param name="password">The password used to derive the decryption key.</param>
    /// <returns>The decrypted plain text.</returns>
    public static string DecryptWithFixedKey(string cipherTextWithIvBase64, string password)
    {
        // Decode the Base64 string back to a byte array
        byte[] cipherTextWithIv = Convert.FromBase64String(cipherTextWithIvBase64);

        using (Aes aes = Aes.Create())
        {
            aes.Key = GetKeyFromPassword(password);

            using (var ms = new MemoryStream(cipherTextWithIv))
            {
                // Read the IV from the beginning of the stream
                byte[] iv = new byte[aes.BlockSize / 8];
                ms.Read(iv, 0, iv.Length);

                using (var decryptor = aes.CreateDecryptor(aes.Key, iv))
                using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                using (var reader = new StreamReader(cs))
                {
                    return reader.ReadToEnd(); // Return the decrypted plain text
                }
            }
        }
    }

    /// <summary>
    /// Generates a 256-bit key from the provided password using SHA-256 hashing.
    /// </summary>
    /// <param name="password">The password used to derive the encryption key.</param>
    /// <returns>A 256-bit encryption key derived from the password.</returns>
    private static byte[] GetKeyFromPassword(string password)
    {
        return SHA256.HashData(Encoding.UTF8.GetBytes(password)); // Return the hashed key
    }
}