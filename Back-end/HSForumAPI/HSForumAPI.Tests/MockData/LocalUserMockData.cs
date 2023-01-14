using HSForumAPI.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HSForumAPI.Tests.MockData
{
    public static class LocalUserMockData
    {
        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new HMACSHA256();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }
        public static List<LocalUser> GetData()
        {
            CreatePasswordHash("test", out byte[] passwordHash, out byte[] passwordSalt);
            return new List<LocalUser>()
            {
                new LocalUser()
                {
                    UserId = 1,
                    Username = "test",
                    Email = "test",
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                    CreatedAt = DateTime.Now
                }
            };
        }
    }
}
