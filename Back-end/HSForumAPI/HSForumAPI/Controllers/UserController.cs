using HSForumAPI.Domain.DTOs.UserDTOs;
using HSForumAPI.Infrastructure.Repositories.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HSForumAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepo;
        public UserController(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }
        [HttpPost("/api/Login")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest req)
        {
            var token = await _userRepo.LoginAsync(req);

            if(token == null) return BadRequest();

            return Ok(new { token = token });
        }
        [HttpPost("/api/Register")]
        [ProducesResponseType(200)]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<LoginResponse>> Register([FromBody] RegistrationRequest req)
        {
            if (await _userRepo.IsRegisteredAsync(req.Username, req.Email)) 
                return BadRequest(new {message = "Username or email already exists" });

            var token = await _userRepo.RegisterAsync(req);

            return Ok(new { token = token });
        }
    }
}
