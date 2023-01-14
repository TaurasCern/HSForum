using HSForumAPI.Domain.DTOs.PostDTOs;
using HSForumAPI.Domain.Enums;
using HSForumAPI.Domain.Services.IServices;
using HSForumAPI.Infrastructure.Repositories.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HSForumAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IUnitOfWork _db;
        private readonly IAdapterService _adapter;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRatingService _ratingService;
        public PostController(
            IUnitOfWork db,
            IAdapterService adapter,
            IHttpContextAccessor httpContextAccessor,
            IRatingService ratingService)
        {
            _db = db;
            _adapter = adapter;
            _httpContextAccessor = httpContextAccessor;
            _ratingService = ratingService;
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Moderator,User")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<PostResponse>> Post(PostRequest request)
        {
            if (!int.TryParse(_httpContextAccessor.HttpContext.User.Identity.Name,
                out int userId))
            {
                return BadRequest();
            }

            var post = _adapter.Bind(request,
                userId);

            var created = _db.Posts.CreateAsync(post);

            return Ok(_adapter.Bind(await created, 0));
        }
        [HttpGet("{type}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<PostResponse>> GetByType([FromRoute] string type)
        {
            var isParsed = Enum.TryParse(typeof(EPostType), type, out object? postType);
            if (!isParsed)
            {
                return BadRequest();
            }
            var posts = await _db.Posts.GetAllAsync(p => p.PostType.Type == (EPostType)postType);

            if (posts == null)
            {
                return NotFound();
            }

            var response = posts.Select(p => _adapter.Bind(p, _ratingService.CalculateRating(p)));
         
            return Ok(response);
        }
        [HttpGet("{id:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<PostResponse>> GetById([FromRoute]int id)
        {
            var post = await _db.Posts.GetWithRepliesAsync(p => p.PostId == id);

            if (post == null)
            {
                return NotFound(new { id });
            }

            var response = _adapter.Bind(post, _ratingService.CalculateRating(post));

            return Ok(response);
        }
    }
}
