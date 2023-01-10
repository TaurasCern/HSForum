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
        private readonly IUnitOfWork _db;
        public UserController(IUnitOfWork db)
        {
            _db = db;
        }
        [HttpPost("/api/Login")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest req)
        {
            var token = await _db.Users.LoginAsync(req);

            if(token == null) return BadRequest();

            return Ok(new { token = token });
        }
        [HttpPost("/api/Register")]
        [ProducesResponseType(200)]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<LoginResponse>> Register([FromBody] RegistrationRequest req)
        {
            if (await _db.Users.IsRegisteredAsync(req.Username, req.Email)) 
                return BadRequest(new {message = "Username or email already exists" });

            var token = await _db.Users.RegisterAsync(req);

            return Ok(new { token = token });
        }
    }
}
