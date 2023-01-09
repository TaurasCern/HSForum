using HSForumAPI.Domain.DTOs;
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
    public class UserRepository : IUserRepository
    {
        private readonly HSForumContext _db;
        private readonly IPasswordService _passwordService;
        private readonly IJwtService _jwtService;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly int _defaultRoleId = 1;
        private readonly ERole _defaultRole = ERole.User;
        public UserRepository(HSForumContext db, IPasswordService passwordService, IJwtService jwtService, IUserRoleRepository userRoleRepository)
        {
            _db = db;
            _passwordService = passwordService;
            _jwtService = jwtService;
            _userRoleRepository = userRoleRepository;
        }
        /// <summary>
        /// Should return a flag indicating if a user with a specified username already exists
        /// </summary>
        /// <param name="username">Registration username</param>
        /// <returns>A flag indicating if username already exists</returns>
        public async Task<bool> IsRegisteredAsync(string username, string email) 
            => await _db.Users.AnyAsync(u => u.Username.ToLower() == username.ToLower()
                                             || u.Email.ToLower() == email.ToLower());
        /// <summary>
        /// Checks if login request is valid and returns the token if valid
        /// </summary>
        /// <param name="loginRequest"></param>
        /// <returns>Token if valid or null if not</returns>
        public async Task<string?> LoginAsync(LoginRequest loginRequest)
        {
            var user = await _db.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Username.ToLower() == loginRequest.Username.ToLower());

            if (user == null)
            {
                return null;
            }

            if(!_passwordService.VerifyPasswordHash(loginRequest.Password, user.PasswordHash, user.PasswordSalt))
            {
                return null;
            }

            return await _jwtService.GetJwtToken(user.UserId,
                user.UserRoles
                    .Select(ur => ur.Role.Name.ToString()).ToArray());
        }
        /// <summary>
        /// Adds new user and Many-to-many UserRole to the database and returns token if successful
        /// </summary>
        /// <param name="registrationRequest"></param>
        /// <returns>Token if valid or null if not</returns>
        public async Task<string?> RegisterAsync(RegistrationRequest registrationRequest)
        {
            _passwordService.CreatePasswordHash(registrationRequest.Password, out byte[] passwordHash, out byte[] passwordSalt);

            LocalUser user = new()
            {
                Username = registrationRequest.Username,
                Email = registrationRequest.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                CreatedAt = DateTime.Now
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            await _userRoleRepository.CreateAsync(new()
            {
                UserId = user.UserId,
                RoleId = _defaultRoleId
            });

            return await _jwtService.GetJwtToken(user.UserId, new string[1] { _defaultRole.ToString() }); ;
        }  
    }
}
