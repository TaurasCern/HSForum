using HSForumAPI.Domain.DTOs.PostDTOs;
using HSForumAPI.Domain.Services.IServices;
using HSForumAPI.Infrastructure.Repositories.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HSForumAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PostController : ControllerBase
    {
        private readonly IUnitOfWork _db;
        private readonly IAdapterService _adapter;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public PostController(
            IUnitOfWork db, 
            IAdapterService adapter, 
            IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _adapter = adapter;
            _httpContextAccessor = httpContextAccessor;
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

            return Ok(_adapter.Bind(await created));
        }
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<PostResponse>> GetByType(PostGetTypeRequest request)
        {
            var posts = await _db.Posts.GetAllAsync(p => p.PostType.Type.ToString() == request.PostType);

            if(posts == null)
            {
                return NotFound(request);
            }

            var response = posts.Select(p => _adapter.Bind(p));

            return Ok(response);
        }
    }
}
