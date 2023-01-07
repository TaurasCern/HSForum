using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSForumAPI.Domain.Services.IServices
{
    public interface IJwtService
    {
        Task<string> GetJwtToken(int userId, string[] roles);
    }
}
