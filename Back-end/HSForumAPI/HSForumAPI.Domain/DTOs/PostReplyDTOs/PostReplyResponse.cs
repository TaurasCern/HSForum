namespace HSForumAPI.Domain.DTOs.PostReplyDTOs
{
    public class PostReplyResponse
    {
        public int ReplyId { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public int UserId { get; set; }
    }
}