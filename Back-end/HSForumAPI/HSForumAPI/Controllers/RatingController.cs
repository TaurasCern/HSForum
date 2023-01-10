using HSForumAPI.Domain.DTOs.RatingDTOs;
using HSForumAPI.Domain.Services.IServices;
using HSForumAPI.Infrastructure.Repositories.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HSForumAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingController : ControllerBase
    {
        private readonly IUnitOfWork _db;
        private readonly IAdapterService _adapter;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public RatingController(
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
        public async Task<ActionResult<RatingResponse>> Post([FromBody]RatingRequest request)
        {
            if (!int.TryParse(_httpContextAccessor.HttpContext.User.Identity.Name,
                    out int userId))
            {
                return BadRequest();
            }

            var rating = _adapter.Bind(request, userId);

            var created = _db.Ratings.CreateAsync(rating);

            return Ok(_adapter.Bind(await created));
        }
    }
}
