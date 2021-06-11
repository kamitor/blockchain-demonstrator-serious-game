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
            RNGCryptoServiceProvider rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            byte[] randomBytes = new byte[64];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            return randomBytes;
        }

        public static Tuple<string, string> ComputeHashWithSalt(string password, string hash = null)
        {
            byte[] saltBytes = GenerateRandomCryptographicBytes();
            byte[] passwordAsBytes = Encoding.UTF8.GetBytes(password);
            List<byte> passwordWithSaltBytes = new List<byte>();
            passwordWithSaltBytes.AddRange(passwordAsBytes);
            passwordWithSaltBytes.AddRange((hash == null) ? saltBytes : Convert.FromBase64String(hash));
            byte[] computedBytes = null;
            using(var sha  = SHA256.Create())
            {
                computedBytes = sha.ComputeHash(passwordWithSaltBytes.ToArray());
            }
            var dsad = Convert.ToBase64String(saltBytes);
            var asds = Convert.FromBase64String(dsad);
            return new Tuple<string,string>(Convert.ToBase64String(computedBytes), Convert.ToBase64String(saltBytes));
        }

        public static bool HashCompare(string password, string passwordHash, string hash)
        {
            return String.Equals(ComputeHashWithSalt(password, hash).Item1, passwordHash);
        }
    }
}
