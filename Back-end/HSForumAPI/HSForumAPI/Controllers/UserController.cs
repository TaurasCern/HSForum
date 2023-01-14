using HSForumAPI.Domain.DTOs.UserDTOs;
using HSForumAPI.Domain.Models;
using HSForumAPI.Domain.Services.IServices;
using HSForumAPI.Infrastructure.Repositories.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HSForumAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUnitOfWork _db;
        private readonly IJwtService _jwtService;
        private readonly IPasswordService _passwordService;
        public UserController(IUnitOfWork db, IJwtService jwtService, IPasswordService passwordService)
        {
            _db = db;
            _jwtService = jwtService;
            _passwordService = passwordService;
        }
        [HttpPost("/api/Login")]
        [ProducesResponseType(200, Type = typeof(LoginResponse))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Login([FromBody] LoginRequest req)
        {
            var result = await _db.Users.TryLoginAsync(req.Username, req.Password);

            if (!result.Item1)
                return Unauthorized();

            var token = _jwtService
                .GetJwtToken(result.Item2.UserId,
                    result.Item2.UserRoles
                        .Select(ur => ur.Role.Name.ToString()).ToArray());
            return Ok(new { token });
        }
        [HttpPost("/api/Register")]
        [ProducesResponseType(200, Type = typeof(LoginResponse))]
        [ProducesResponseType(201)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Register([FromBody] RegistrationRequest req)
        {
            if (await _db.Users.IsRegisteredAsync(req.Username, req.Email))
                return BadRequest("User already exists");

            _passwordService.CreatePasswordHash(req.Password, out var passwordHash, out var passwordSalt);

            var user = new LocalUser
            {
                Username = req.Username,
                Email = req.Email,
                CreatedAt = DateTime.Now,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };

            var id = await _db.Users.RegisterAsync(user);

            return Created(nameof(Login), new { id });
        }
    }
}
