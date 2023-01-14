using HSForumAPI.Domain.DTOs.UserDTOs;
using HSForumAPI.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSForumAPI.Infrastructure.Repositories.IRepositories
{
    public interface IUserRepository : IRepository<LocalUser>
    {
        Task<Tuple<bool, LocalUser?>> TryLoginAsync(string username, string password);
        Task<int> RegisterAsync(LocalUser user);
        Task<bool> IsRegisteredAsync(string username, string email);
    }
}
