using HSForumAPI.Domain.DTOs.PostDTOs;
using HSForumAPI.Domain.DTOs.PostReplyDTOs;
using HSForumAPI.Domain.DTOs.UserDTOs;
using HSForumAPI.Domain.Enums;
using HSForumAPI.Domain.Models;
using HSForumAPI.Domain.Services.IServices;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSForumAPI.Domain.Services
{
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

        public PostResponse Bind(Post post) => new()
        {
            PostId = post.PostId,
            Title = post.Title,
            Content = post.Content,
            CreatedAt = post.CreatedAt,
            UserId = post.UserId,
            Replies = post.Replies == null ? null 
                : post.Replies
                    .Select(r => Bind(r))
                        .ToArray()       
        };

        public PostReplyResponse Bind(PostReply reply) => new()
        {
            ReplyId = reply.ReplyId,
            Content = reply.Content,
            CreatedAt = reply.CreatedAt,
            UserId = reply.UserId
        };

        public PostReply Bind(PostReplyRequest request, int userId) => new()
        {
            Content = request.Content,
            CreatedAt = DateTime.Now,
            PostId = request.PostId,
            UserId = userId
        };
    }
}
