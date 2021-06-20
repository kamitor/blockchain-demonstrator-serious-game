using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BlockchainDemonstratorApi.Models.Classes
{
    /// <summary>
    /// The Cryptography class contains methods to generate and compare hashes.
    /// This class is mainly used for the protection of the admin password.
    /// </summary>
    public static class Cryptography
    {
        /// <summary>
        /// This function is used to create a random array of bytes with a length of 64.
        /// This function uses the RNGCryptoServiceProvider instead of the Random class because it creates a more secury random array of bytes.
        /// </summary>
        /// <returns>Returns a random array of bytes with a length of 64</returns>
        private static byte[] GenerateRandomCryptographicBytes()
        {
            byte[] randomBytes = new byte[64];
            using (RNGCryptoServiceProvider rngCryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                rngCryptoServiceProvider.GetBytes(randomBytes);
            }
            return randomBytes;
        }

        /// <summary>
        /// This is the main method of the Cryptography class as it computes the password hash.
        /// It does this by creating two arrays of bytes, one is the salt, the other the password.
        /// These two arrays are added together to create one array which will be used in the hashing algorithm.
        /// The hashing algorithm computes a new array of bytes from the merged aray. The algorithm used in this case is SHA256.
        /// The computed hash is returned together with the salt in a Base64 string format.
        /// </summary>
        /// <param name="password">The password parameter is the password the user has inputted.</param>
        /// <param name="salt">The salt parameter is a optional parameter used to compute a determinable hash</param>
        /// <returns>A tuple which contains the computed hash and salt</returns>
        /// <remarks>The salt parameter is optional so this method can be used to both create a new hash and determinable hash</remarks>
        public static Tuple<string, string> ComputeHashWithSalt(string password, string salt = null)
        {
            byte[] saltBytes = GenerateRandomCryptographicBytes();
            byte[] passwordAsBytes = Encoding.UTF8.GetBytes(password);
            List<byte> passwordWithSaltBytes = new List<byte>();
            passwordWithSaltBytes.AddRange(passwordAsBytes);
            passwordWithSaltBytes.AddRange((salt == null) ? saltBytes : Convert.FromBase64String(salt));
            byte[] computedBytes = null;
            using(var sha  = SHA256.Create())
            {
                computedBytes = sha.ComputeHash(passwordWithSaltBytes.ToArray());
            }
            return new Tuple<string,string>(Convert.ToBase64String(computedBytes), Convert.ToBase64String(saltBytes));
        }

        /// <summary>
        /// This method is used to compare the inputted password by the user with the computed password hash in the database.
        /// </summary>
        /// <param name="password">The password inserted by the user</param>
        /// <param name="passwordHash">The computed password hash in the database</param>
        /// <param name="salt">The salt coupled with the password hash</param>
        /// <returns>A boolean which states whether the hashes are equal or not</returns>
        public static bool HashCompare(string password, string passwordHash, string salt)
        {
            return String.Equals(ComputeHashWithSalt(password, salt).Item1, passwordHash);
        }
    }
}
