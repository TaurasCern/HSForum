using HSForumAPI.Domain.Models;
using HSForumAPI.Domain.Services.IServices;

namespace HSForumAPI.Domain.Services
{
    public class RatingService : IRatingService
    {
        /// <summary>
        /// Calculates rating using Post=>Rating
        /// </summary>
        /// <param name="post"></param>
        /// <returns>Rating count</returns>
        public async Task<int> CalculateRating(Post post)
            => post.Ratings == null ? 0 
                : post.Ratings.Select(r => r.IsPositive ? 1 : -1)
                    .Sum();
        /// <summary>
        /// Calculates total rating out of given posts
        /// </summary>
        /// <param name="posts"></param>
        /// <returns></returns>
        public async Task<int> CalculateUserReputation(List<Post> posts)
        {
            var reputation = 0;
            foreach (var post in posts)
            {
                reputation += await CalculateRating(post);
            }
            return reputation;
        }
        /// <summary>
        /// Check if post was rated by the given user id
        /// </summary>
        /// <param name="post"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<bool> CheckIfRated(Post post, int userId) 
            => post.Ratings != null 
            && post.Ratings
                .Any(r => r.UserId == userId);
    }
}
