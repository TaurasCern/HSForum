using HSForumAPI.Domain.DTOs.UserDTOs;
using HSForumAPI.Domain.Models;
using HSForumAPI.Domain.Services.IServices;
using HSForumAPI.Infrastructure.Repositories.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HSForumAPI.Controllers
{
    /// <summary>
    /// Controller handling user endpoints
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUnitOfWork _db;
        private readonly IJwtService _jwtService;
        private readonly IPasswordService _passwordService;
        private readonly IAdapterService _adapter;
        private readonly IRatingService _ratingService;
        public UserController(
            IUnitOfWork db, 
            IJwtService jwtService, 
            IPasswordService passwordService, 
            IAdapterService adapterService,
            IRatingService ratingService)
        {
            _db = db;
            _jwtService = jwtService;
            _passwordService = passwordService;
            _adapter = adapterService;
            _ratingService = ratingService;
        }
        /// <summary>
        /// Login request
        /// </summary>
        /// <param name="req">Username, password</param>
        /// <returns>Token</returns>
        /// <response code="401">Wrong password or user doesn't exist</response>
        /// <remarks>
        /// Sample:    
        ///     POST Login
        ///     {
        ///         "Username": "sampleUsername",
        ///         "Password": "samplePassword",
        ///     }
        /// </remarks>
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

            var response = _adapter.Bind(result.Item2);

            var token = _jwtService
                .GetJwtToken(result.Item2.UserId, response.Roles);

            response.Token = token;

            return Ok(response);
        }
        /// <summary>
        /// Registration request
        /// </summary>
        /// <param name="req">RegistrationRequest</param>
        /// <returns>Id of the created user</returns>
        /// <response code="400">User already exists</response>
        /// <remarks>
        /// Sample:    
        ///     POST Register
        ///     {
        ///         "Username": "sampleUsername",
        ///         "Email": "sampleEmail",
        ///         "Password": "samplePassword"
        ///     }
        /// </remarks>
        [HttpPost("/api/Register")]
        [ProducesResponseType(200)]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
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
        /// <summary>
        /// Fetches user with specified ID from DB
        /// </summary>
        /// <param name="id">Requested user Id</param>
        /// <returns>User of the specified Id</returns>
        /// <response code="400">Bad route</response>
        /// <response code="404">User was not found</response>
        /// <remarks>
        /// Sample:    
        ///     POST User/1
        /// </remarks>
        [HttpGet("{id:int}")]
        [ProducesResponseType(200, Type = typeof(UserGetResponse))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Get([FromRoute]int id)
        {
            if(id == 0)
            {
                return BadRequest();
            }

            if(!await _db.Users.ExistAsync(u => u.UserId == id))
            {
                return NotFound();
            }

            var user = await _db.Users.GetAsync(u => u.UserId == id);

            var userPosts = await _db.Posts.GetAllWithRatingsAsync(p => p.UserId == user.UserId);

            var response = _adapter.Bind(user, await _ratingService.CalculateUserReputation(userPosts)); 

            return Ok(response);
        }
    }
}
