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
        public async Task<IActionResult> Post(PostRequest request)
        {
            var post = _adapter.Bind(request,
                int.Parse(_httpContextAccessor.HttpContext.User.Identity.Name));

            var created = _db.Posts.CreateAsync(post);

            return Ok(await created);
        }
    }
}
