namespace HSForumAPI.Domain.DTOs.PostReplyDTOs
{
    public class PostReplyResponse
    {
        /// <summary>
        /// Id of the reply
        /// </summary>
        public int ReplyId { get; set; }
        /// <summary>
        /// Contents of the reply
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// Reply creation date
        /// </summary>
        public DateTime CreatedAt { get; set; }
        /// <summary>
        /// Id of the user who made the reply
        /// </summary>
        public int UserId { get; set; }
    }
}