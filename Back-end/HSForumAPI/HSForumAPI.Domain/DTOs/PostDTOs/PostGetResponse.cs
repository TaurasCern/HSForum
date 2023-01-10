using HSForumAPI.Domain.DTOs.PostReplyDTOs;

namespace HSForumAPI.Domain.DTOs.PostDTOs
{
    public class PostGetResponse
    {
        /// <summary>
        /// Id of the post
        /// </summary>
        public int PostId { get; set; }
        /// <summary>
        /// Title of the post
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Contents of the post
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// Post creation date
        /// </summary>
        public DateTime CreatedAt { get; set; }
        /// <summary>
        /// UserId of the user who made the post
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// Comments on the post
        /// </summary>
        public PostReplyResponse[] Replies { get; set; }
    }
}
