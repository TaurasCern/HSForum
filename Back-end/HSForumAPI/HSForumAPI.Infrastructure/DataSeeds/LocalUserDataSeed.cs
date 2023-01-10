using HSForumAPI.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HSForumAPI.Infrastructure.DataSeeds
{
    public static class LocalUserDataSeed
    {
        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new HMACSHA256();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }
        public static LocalUser GetData()
        {
            CreatePasswordHash("admin123", out byte[] passwordHash, out byte[] passwordSalt);
            return new()
            {
                UserId = 1,
                Username = "Admin",
                Email = "admin@hsforum.lt",
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                CreatedAt = DateTime.Now
            };
        }
    }
}
