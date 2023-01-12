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
        private readonly IRatingService _ratingService;
        public RatingController(
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
        [ProducesResponseType(201)]
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
            var post = await _db.Posts.GetWithRepliesAsync(p => p.PostId == request.PostId);
            
            if(_ratingService.CheckIfRated(post, userId))
            {
                var existingRating = await _db.Ratings.GetAsync(r => r.UserId == userId && r.PostId == post.PostId);

                if(existingRating.IsPositive == request.IsPositive)
                {
                    return Ok(_adapter.Bind(existingRating, wasAltered: false));
                }

                existingRating.IsPositive = request.IsPositive; 

                var updated =  await _db.Ratings.UpdateAsync(existingRating);

                return Ok(_adapter.Bind(updated, wasAltered: true));
            }

            var rating = _adapter.Bind(request, userId);

            var created = _db.Ratings.CreateAsync(rating);

            return Created("",_adapter.Bind(await created, wasAltered: false));
        }
    }
}
