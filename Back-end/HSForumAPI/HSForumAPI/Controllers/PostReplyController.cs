using HSForumAPI.Domain.DTOs.PostReplyDTOs;
using HSForumAPI.Domain.Services.IServices;
using HSForumAPI.Infrastructure.Repositories.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HSForumAPI.Controllers
{
    /// <summary>
    /// Controller to handle PostReply endpoints
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PostReplyController : ControllerBase
    {
        private readonly IUnitOfWork _db;
        private readonly IAdapterService _adapter;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<PostReplyController> _logger;

        public PostReplyController(
            IUnitOfWork db,
            IAdapterService adapter,
            IHttpContextAccessor httpContextAccessor,
            ILogger<PostReplyController> logger)
        {
            _db = db;
            _adapter = adapter;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }
        /// <summary>
        /// Adds new comment to the database
        /// </summary>
        /// <param name="request">PostReplyRequest</param>
        /// <response code="401">You have to be logged in</response>
        /// <remarks>
        /// Sample:    
        ///     POST PostReply
        ///     {
        ///         "content": "sampleContent",
        ///         "postId": 1
        ///     }
        /// </remarks>
        [HttpPost]
        [Authorize(Roles = "Admin,Moderator,User")]
        [ProducesResponseType(200, Type = typeof(PostReplyResponse))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<PostReplyResponse>> Post([FromBody]PostReplyRequest request)
        {
            if(request == null)
            {
                return BadRequest();
            }

            var userId = int.Parse(_httpContextAccessor.HttpContext.User.Identity.Name);

            var postReply = _adapter.Bind(request, userId);

            var created = _db.PostReplies.CreateAsync(postReply);

            var user = await _db.Users.GetAsync(u => u.UserId == userId);

            return Ok(_adapter.Bind(await created, user.Username));
        }
        /// <summary>
        /// Deactivates the post reply by given Id.
        /// Admin and Moderator roles can deactivate any post.
        /// User only can deactivate its own post
        /// </summary>
        /// <param name="id">Id of the post reply</param>
        /// <response code="401">You have to be logged in</response>
        /// <response code="403">User role can only deactivate its own posts</response>
        /// <response code="404">PostReply was not found</response>
        /// <remarks>
        /// Sample:    
        ///     DELETE PostReply/1
        /// </remarks>
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin,Moderator,User")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Deactivate([FromRoute] int id)
        {
            var roles = _httpContextAccessor.HttpContext.User.Claims
                .Where(c => c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role")
                .Select(c => c.Value);

            if (!await _db.PostReplies.ExistAsync(p => p.ReplyId == id))
            {
                return NotFound();
            }

            var postReply = await _db.PostReplies.GetAsync(p => p.ReplyId == id);

            var userId = int.Parse(_httpContextAccessor.HttpContext.User.Identity.Name);

            if (roles.Contains("Admin") || roles.Contains("Moderator"))
            {
                postReply.IsActive = false;
                await _db.PostReplies.UpdateAsync(postReply);
                _logger.LogInformation("User: {userId} deactivated PostReply: {postId}", userId, postReply.ReplyId);
                return Ok();
            }

            if (postReply.UserId != userId)
            {
                return Forbid();
            }

            postReply.IsActive = false;
            await _db.PostReplies.UpdateAsync(postReply);
            _logger.LogInformation("User: {userId} deactivated PostReply: {postId}", userId, postReply.ReplyId);

            return Ok();
        }
    }
}
