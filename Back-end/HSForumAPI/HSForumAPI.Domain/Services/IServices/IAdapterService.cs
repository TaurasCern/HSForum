﻿using HSForumAPI.Domain.DTOs.PostDTOs;
using HSForumAPI.Domain.DTOs.PostReplyDTOs;
using HSForumAPI.Domain.DTOs.PostTypeDTOs;
using HSForumAPI.Domain.DTOs.RatingDTOs;
using HSForumAPI.Domain.DTOs.UserDTOs;
using HSForumAPI.Domain.Models;

namespace HSForumAPI.Domain.Services.IServices
{
    public interface IAdapterService
    {
        Post Bind(PostRequest request, int userId);
        PostResponse Bind(Post post, int rating);
        PostReplyResponse Bind(PostReply reply);
        PostReplyResponse Bind(PostReply reply, string username);
        PostReply Bind(PostReplyRequest request, int userId);
        Rating Bind(RatingRequest request, int userId);
        RatingResponse Bind(Rating rating, bool wasAltered);
        PostUpdateRequest Bind(Post post);
        Post Bind(PostUpdateRequest request, int postId, int postTypeId, int userId, bool isActive);
        UserGetResponse Bind(LocalUser user, int reputation, int postCount);
        LoginResponse Bind(LocalUser user);
        PostTypeResponse Bind(PostType postType);
    }
}
