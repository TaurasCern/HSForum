using HSForumAPI.Domain.DTOs.PostDTOs;
using HSForumAPI.Domain.DTOs.PostReplyDTOs;
using HSForumAPI.Domain.Models;

namespace HSForumAPI.Domain.Services.IServices
{
    public interface IAdapterService
    {
        Post Bind(PostRequest request, int userId);
        PostResponse Bind(Post post);
        PostReplyResponse Bind(PostReply reply);
        PostReply Bind(PostReplyRequest request);
    }
}
