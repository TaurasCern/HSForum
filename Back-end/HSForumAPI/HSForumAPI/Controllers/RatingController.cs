using HSForumAPI.Domain.DTOs.RatingDTOs;
using HSForumAPI.Domain.Services.IServices;
using HSForumAPI.Infrastructure.Repositories.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HSForumAPI.Controllers
{
    /// <summary>
    /// Controller to handle Rating endpoints
    /// </summary>
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
        /// <summary>
        /// Adds new rating to the database or updates, 
        /// if there's an existing rating by the user
        /// </summary>
        /// <param name="request">RatingRequest</param>
        /// <response code="400">Bad request body</response>
        /// <response code="401">You have to be logged in</response>
        /// <remarks>
        /// Sample:    
        ///     POST Rating
        ///     {
        ///         "isPositive": true,
        ///         "postId": 1
        ///     }
        /// </remarks>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(200, Type = typeof(RatingResponse))]
        [ProducesResponseType(201, Type = typeof(RatingResponse))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<RatingResponse>> Post([FromBody]RatingRequest request)
        {
            if(request == null)
            {
                return BadRequest();
            }
            var userId = int.Parse(_httpContextAccessor.HttpContext.User.Identity.Name);

            var post = await _db.Posts.GetWithRepliesAsync(p => p.PostId == request.PostId);
            
            if(await _ratingService.CheckIfRated(post, userId))
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

            return Created(nameof(Post),_adapter.Bind(await created, wasAltered: false));
        }
    }
}
