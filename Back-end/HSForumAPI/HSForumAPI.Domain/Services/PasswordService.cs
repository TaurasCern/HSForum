using HSForumAPI.Domain.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HSForumAPI.Domain.Services
{
    public class PasswordService : IPasswordService
    {
        /// <summary>
        /// Creates Hash and Salt for the given password
        /// </summary>
        /// <param name="password">password of the user</param>
        /// <returns>Tuple Item 1 -> Hash</returns>
        /// <returns>Tuple Item 2 -> Salt</returns>
        public Tuple<byte[], byte[]> CreatePasswordHash(string password)
        {
            using var hmac = new HMACSHA256();
            var passwordSalt = hmac.Key;
            var passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

            return new(passwordHash, passwordSalt);
        }
        /// <summary>
        /// Checks if the password is equal to the hashed password
        /// </summary>
        /// <param name="password"></param>
        /// <param name="passwordHash"></param>
        /// <param name="passwordSalt"></param>
        /// <returns>True if equal</returns>
        public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using var hmac = new HMACSHA256(passwordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

            return computedHash.SequenceEqual(passwordHash);
        }
    }
}
