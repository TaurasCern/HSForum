using HSForumAPI.Domain.Models;
using HSForumAPI.Domain.Services.IServices;

namespace HSForumAPI.Domain.Services
{
    public class RatingService : IRatingService
    {
        public int CalculateRating(Post post)
            => post.Ratings == null ? 0 
                : post.Ratings.Select(r => r.IsPositive ? 1 : -1)
                    .Sum();

        public bool CheckIfRated(Post post, int userId) 
            => post.Ratings == null ? false 
                : post.Ratings.Any(r => r.UserId == userId);
    }
}
