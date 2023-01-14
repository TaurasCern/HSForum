using HSForumAPI.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSForumAPI.Domain.Services.IServices
{
    public interface IRatingService
    {
        Task<int> CalculateRating(Post post);
        Task<bool> CheckIfRated(Post post, int userId);
        Task<int> CalculateUserReputation(List<Post> userPosts);
    }
}
