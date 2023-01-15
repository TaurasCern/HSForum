namespace HSForumAPI.Domain.Models
{
    public class PostReply
    {
        public int ReplyId { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }

        public int PostId { get; set; }
        public Post Post { get; set; }
        public int UserId { get; set; }
        public LocalUser User { get; set; }
        public bool IsActive { get; set; }
    }
}