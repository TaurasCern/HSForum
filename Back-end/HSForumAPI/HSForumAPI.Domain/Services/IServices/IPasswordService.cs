using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSForumAPI.Domain.Services.IServices
{
    public interface IPasswordService
    {
        Tuple<byte[], byte[]> CreatePasswordHash(string password);
        bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);
    }
}
