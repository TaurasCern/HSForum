using HSForumAPI.Domain.Models;
using HSForumAPI.Domain.Services.IServices;

namespace HSForumAPI.Domain.Services
{
    public class RatingService : IRatingService
    {
        public async Task<int> CalculateRating(Post post)
            => post.Ratings == null ? 0 
                : post.Ratings.Select(r => r.IsPositive ? 1 : -1)
                    .Sum();

        public async Task<int> CalculateUserReputation(List<Post> posts)
        {
            var reputation = 0;
            foreach (var post in posts)
            {
                reputation += await CalculateRating(post);
            }
            return reputation;
        }

        public async Task<bool> CheckIfRated(Post post, int userId) 
            => post.Ratings != null 
            && post.Ratings
                .Any(r => r.UserId == userId);
    }
}
