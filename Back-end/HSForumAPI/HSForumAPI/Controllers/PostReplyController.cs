using HSForumAPI.Domain.DTOs.PostReplyDTOs;
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
        [HttpPost]
        [Authorize(Roles = "Admin,Moderator,User")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<PostReplyResponse>> Post(PostReplyRequest request)
        {
            var postReply = _adapter.Bind(request);

            var created = _db.PostReplies.CreateAsync(postReply);

            return Ok(_adapter.Bind(await created));
        }

    }
}
