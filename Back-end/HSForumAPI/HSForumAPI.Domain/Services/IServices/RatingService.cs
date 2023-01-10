using HSForumAPI.Domain.Models;

namespace HSForumAPI.Domain.Services.IServices
{
    public class RatingService : IRatingService
    {
        public async Task<int> CalculateRating(Post post) 
            => post.Ratings.Select(r => r.IsPositive ? 1 : -1).Sum();
    }
}
