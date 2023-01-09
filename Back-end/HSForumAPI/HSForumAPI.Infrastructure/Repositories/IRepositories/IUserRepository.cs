using HSForumAPI.Domain.DTOs.UserDTOs;
using HSForumAPI.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSForumAPI.Infrastructure.Repositories.IRepositories
{
    public interface IUserRepository
    {
        Task<string?> LoginAsync(LoginRequest loginRequest);
        Task<string?> RegisterAsync(RegistrationRequest registrationRequest);
        Task<bool> IsRegisteredAsync(string username, string email);
    }
}
