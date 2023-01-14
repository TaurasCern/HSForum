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
        public PostReplyController(
            IUnitOfWork db,
            IAdapterService adapter,
            IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _adapter = adapter;
            _httpContextAccessor = httpContextAccessor;
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

            return Ok(_adapter.Bind(await created));
        }

    }
}
