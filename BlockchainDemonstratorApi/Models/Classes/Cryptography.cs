using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BlockchainDemonstratorApi.Models.Classes
{
    public static class Cryptography
    {
        private static byte[] GenerateRandomCryptographicBytes()
        {
            byte[] randomBytes = new byte[64];
            using (RNGCryptoServiceProvider rngCryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                rngCryptoServiceProvider.GetBytes(randomBytes);
            }
            return randomBytes;
        }

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

        public static bool HashCompare(string password, string passwordHash, string hash)
        {
            return String.Equals(ComputeHashWithSalt(password, hash).Item1, passwordHash);
        }
    }
}
