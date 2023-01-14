using HSForumAPI.Domain.DTOs.UserDTOs;
using HSForumAPI.Domain.Enums;
using HSForumAPI.Domain.Models;
using HSForumAPI.Domain.Services.IServices;
using HSForumAPI.Infrastructure.Database;
using HSForumAPI.Infrastructure.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSForumAPI.Infrastructure.Repositories
{
    public class UserRepository : Repository<LocalUser>, IUserRepository
    {
        private readonly HSForumContext _db;
        private readonly IPasswordService _passwordService;
        private readonly IUserRoleRepository _userRoleRepository;

        private readonly int _defaultRoleId = 1;
        public UserRepository(HSForumContext db, 
            IPasswordService passwordService,
            IUserRoleRepository userRoleRepository)
            : base(db)
        {
            _db = db;
            _passwordService = passwordService;
            _userRoleRepository = userRoleRepository;
        }
        /// <summary>
        /// Should return a flag indicating if a user with a specified username already exists
        /// </summary>
        /// <param name="username">Registration username</param>
        /// <returns>A flag indicating if username already exists</returns>
        public async Task<bool> IsRegisteredAsync(string username, string email) 
            => await _db.Users
                        .AnyAsync(u => u.Username.ToLower() == username.ToLower()
                                    || u.Email.ToLower() == email.ToLower());
        /// <summary>
        /// Checks if login request is valid and returns tuple with boolean if it was success and user
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="user"></param>
        /// <returns>Tuple bool Is successful and user</returns>
        public async Task<Tuple<bool,LocalUser?>> TryLoginAsync(string username, string password)
        {
             var user = await _db.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Username.ToLower() == username.ToLower());

            if (user == null)
                return new (false, user);

            if(!_passwordService.VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return new (false, user);

            return new (true, user);
        }
        /// <summary>
        /// Adds new user return id
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Id</returns>
        public async Task<int> RegisterAsync(LocalUser user)
        {
            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            await _userRoleRepository.CreateAsync(new()
            {
                UserId = user.UserId,
                RoleId = _defaultRoleId
            });

            await _db.SaveChangesAsync();
            return user.UserId;
        }
    }
}
