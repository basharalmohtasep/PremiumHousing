using System.Security.Cryptography;
using System.Text;

namespace PremiumHousing.Helper
{
    public static class HashHelper
    {
        /// <summary>
        /// Hashes the provided password using SHA-256 and returns the hash as a hexadecimal string.
        /// </summary>
        /// <param name="password">The plain text password to hash.</param>
        /// <returns>A SHA-256 hashed representation of the password in hexadecimal format.</returns>
        public static string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
          
        }
    }
}
