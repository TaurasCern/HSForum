using HSForumAPI.Domain.DTOs.PostDTOs;
using HSForumAPI.Domain.Enums;
using HSForumAPI.Domain.Services.IServices;
using HSForumAPI.Infrastructure.Repositories.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace HSForumAPI.Controllers
{
    /// <summary>
    /// Controller handling Post class endpoints
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IUnitOfWork _db;
        private readonly IAdapterService _adapter;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRatingService _ratingService;
        private readonly ILogger<PostController> _logger;
        public PostController(
            IUnitOfWork db,
            IAdapterService adapter,
            IHttpContextAccessor httpContextAccessor,
            IRatingService ratingService,
            ILogger<PostController> logger)
        {
            _db = db;
            _adapter = adapter;
            _httpContextAccessor = httpContextAccessor;
            _ratingService = ratingService;
            _logger = logger;
        }
        /// <summary>
        /// Post class request
        /// </summary>
        /// <param name="request">PostRequest</param>
        /// <returns>Returns the created post</returns>
        /// <response code="401">You have to be logged in to post</response>
        /// <remarks>
        /// PostType can only be (case sensitive):
        /// 1. News
        /// 
        /// Sample:    
        ///     POST Post
        ///     {
        ///         "title": "sampleTitle",
        ///         "content": "sampleContent",
        ///         "postType": "News" 
        ///     }
        /// </remarks>
        [HttpPost]
        [Authorize(Roles = "Admin,Moderator,User")]
        [ProducesResponseType(200, Type = typeof(PostResponse))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Post([FromBody]PostRequest request)
        {
            var userId = int.Parse(_httpContextAccessor.HttpContext.User.Identity.Name);

            var post = _adapter.Bind(request, userId);

            var created = _db.Posts.CreateAsync(post);

            return Ok(_adapter.Bind(await created, 0));
        }
        /// <summary>
        /// Fetches all posts of the given type
        /// </summary>
        /// <param name="type">Type of the post</param>
        /// <returns>Posts of the given type</returns>
        /// <response code="400">PostType wasn't in correct format</response>
        /// <response code="401">No posts were found of the givem type</response>
        /// <remarks>
        /// PostType can only be (case sensitive):
        /// 1. News
        /// 
        /// Sample:    
        ///     GET Post/News
        /// </remarks>
        [HttpGet("{type}")]
        [ProducesResponseType(200, Type = typeof(List<PostResponse>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetByType([FromRoute] string type)
        {
            var isParsed = Enum.TryParse(typeof(EPostType), type, out object? postType);
            if (!isParsed)
            {
                return BadRequest();
            }
            var posts = await _db.Posts.GetAllAsync(p => p.PostType.Type == (EPostType)postType && p.IsActive == true);

            if (posts == null)
            {
                return NotFound();
            }

            var response = new List<PostResponse>();

            foreach (var post in posts)
            {
                response.Add(_adapter.Bind(post, await _ratingService.CalculateRating(post)));
            }
         
            return Ok(response);
        }
        /// <summary>
        /// Fetches post by given Id
        /// </summary>
        /// <param name="id">Id of the post</param>
        /// <returns>Posts of the Id</returns>
        /// <response code="404">Post was not found</response>
        /// <remarks>
        /// Sample:    
        ///     GET Post/1
        /// </remarks>
        [HttpGet("{id:int}")]
        [ProducesResponseType(200, Type = typeof(PostResponse))]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetById([FromRoute]int id)
        {
            var post = await _db.Posts.GetWithRepliesAsync(p => p.PostId == id && p.IsActive == true);

            if (post == null)
            {
                return NotFound(new { id });
            }

            var response = _adapter.Bind(post, await _ratingService.CalculateRating(post));

            return Ok(response);
        }
        /// <summary>
        /// Deactivates the post by given Id.
        /// Admin and Moderator roles can deactivate any post.
        /// User only can deactivate its own post
        /// </summary>
        /// <param name="id">Id of the post</param>
        /// <response code="401">You have to be logged in</response>
        /// <response code="403">User role can only deactivate its own posts</response>
        /// <response code="404">Post was not found</response>
        /// <remarks>
        /// Sample:    
        ///     DELETE Post/1
        /// </remarks>
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin,Moderator,User")]
        [ProducesResponseType(200, Type = typeof(PostResponse))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Deactivate([FromRoute] int id)
        {
            var roles = _httpContextAccessor.HttpContext.User.Claims
                .Where(c => c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role")
                .Select(c => c.Value);

            if (!await _db.Posts.ExistAsync(p => p.PostId == id))
            {
                return NotFound();
            }

            var post = await _db.Posts.GetAsync(p => p.PostId == id);

            if (roles.Contains("Admin") || roles.Contains("Moderator"))
            {
                post.IsActive = false;
                await _db.Posts.UpdateAsync(post);
                return Ok();
            }

            var userId = int.Parse(_httpContextAccessor.HttpContext.User.Identity.Name);

            if(post.UserId != userId)
            {
                return Forbid();
            }

            post.IsActive = false;
            await _db.Posts.UpdateAsync(post);

            return Ok();
        }

        /// <summary>
        /// Patches post by given Id.
        /// User only can edit its own post
        /// </summary>
        /// <param name="id">Id of the post</param>
        /// <param name="request"></param>
        /// <response code="401">You have to be logged in</response>
        /// <response code="403">User role can only edit its own posts</response>
        /// <response code="404">Post was not found</response>
        /// <remarks>
        /// Sample:    
        ///     PATCH Post/Patch/1
        ///     [
        ///         {
        ///             "op": "replace",
        ///             "path": "title",
        ///             "value": "updated"
        ///         },
        ///         {
        ///             "op": "replace",
        ///             "path": "content",
        ///             "value": "updated"
        ///         }
        ///     ]
        /// </remarks>
        [HttpPatch("Patch/{id:int}")]
        [Authorize]
        [ProducesResponseType(200, Type = typeof(PostResponse))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Patch([FromRoute] int id, [FromBody] JsonPatchDocument<PostUpdateRequest> request)
        {
            if (id == 0 || request == null)
            {
                return BadRequest();
            }

            if (!await _db.Posts.ExistAsync(p => p.PostId == id && p.IsActive == true))
            {
                return NotFound();
            }

            var userId = int.Parse(_httpContextAccessor.HttpContext.User.Identity.Name);

            var foundPost = await _db.Posts.GetAsync(p => p.PostId == id, tracked: false);

            if (foundPost.UserId != userId)
            {
                return Forbid();
            }

            var updateRequest = _adapter.Bind(foundPost);

            request.ApplyTo(updateRequest, ModelState);

            var post = _adapter.Bind(updateRequest, foundPost.PostId, foundPost.PostTypeId, foundPost.UserId);

            await _db.Posts.UpdateAsync(post);

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            _logger.LogInformation("Post: {PostId} was edited by User: {UserId}", post.PostId, userId);

            return Ok();
        }
    }
}
