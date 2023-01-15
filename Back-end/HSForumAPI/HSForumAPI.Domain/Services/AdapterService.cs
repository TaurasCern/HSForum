using HSForumAPI.Domain.DTOs.PostDTOs;
using HSForumAPI.Domain.DTOs.PostReplyDTOs;
using HSForumAPI.Domain.DTOs.PostTypeDTOs;
using HSForumAPI.Domain.DTOs.RatingDTOs;
using HSForumAPI.Domain.DTOs.UserDTOs;
using HSForumAPI.Domain.Enums;
using HSForumAPI.Domain.Models;
using HSForumAPI.Domain.Services.IServices;

namespace HSForumAPI.Domain.Services
{
    /// <summary>
    /// Class to convert To or from DTO
    /// </summary>
    public class AdapterService : IAdapterService
    {
        public Post Bind(PostRequest request, int userId) => new()
        {
            Title = request.Title,
            Content = request.Content,
            CreatedAt = DateTime.Now,
            PostTypeId = (int)(EPostType)Enum.Parse(typeof(EPostType), request.PostType),
            IsActive = true,
            UserId = userId
        };

        public PostResponse Bind(Post post, int rating) => new()
        {
            PostId = post.PostId,
            Title = post.Title,
            Content = post.Content,
            CreatedAt = post.CreatedAt,
            UserId = post.UserId,
            Replies = post.Replies?.Select(r => Bind(r))
                        .ToArray(),
            Rating = rating
        };
        public PostReplyResponse Bind(PostReply reply) => new()
        {
            ReplyId = reply.ReplyId,
            Content = reply.Content,
            CreatedAt = reply.CreatedAt,
            UserId = reply.UserId,
            Username = reply.User == null ? null : reply.User.Username
        };
        public PostReplyResponse Bind(PostReply reply, string username) => new()
        {
            ReplyId = reply.ReplyId,
            Content = reply.Content,
            CreatedAt = reply.CreatedAt,
            UserId = reply.UserId,
            Username = username
        };

        public PostReply Bind(PostReplyRequest request, int userId) => new()
        {
            Content = request.Content,
            CreatedAt = DateTime.Now,
            PostId = request.PostId,
            UserId = userId,
            IsActive = true,
        };

        public Rating Bind(RatingRequest request, int userId) => new()
        {
            IsPositive = request.IsPositive,
            PostId = request.PostId,
            UserId = userId
        };

        public RatingResponse Bind(Rating rating, bool wasAltered) => new()
        {
            IsPositive = rating.IsPositive,
            PostId = rating.PostId,
            UserId = rating.UserId,
            WasAltered = wasAltered
        };

        public PostUpdateRequest Bind(Post post) => new()
        {
            Title = post.Title,
            Content = post.Content,
            CreatedAt = post.CreatedAt
        };

        public Post Bind(PostUpdateRequest request, int postId, int postTypeId, int userId, bool isActive) => new()
        {
            PostId = postId,
            Title = request.Title,
            Content = request.Content,
            CreatedAt = request.CreatedAt,
            PostTypeId = postTypeId,
            UserId = userId,
            IsActive = isActive
        };

        public UserGetResponse Bind(LocalUser user, int reputation, int postCount) => new()
        {
            Username = user.Username,
            CreatedAt = user.CreatedAt,
            Reputation = reputation,
            PostCount = postCount
        };

        public LoginResponse Bind(LocalUser user) => new()
        {
            UserId = user.UserId,
            Roles = user.UserRoles
                        .Select(ur => ur.Role.Name.ToString())
                            .ToArray()
        };

        public PostTypeResponse Bind(PostType postType) => new()
        {
            Type = postType.Type.ToString()
        };
    }
}
