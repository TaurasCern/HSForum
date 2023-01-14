namespace HSForumAPI.Domain.DTOs.UserDTOs
{
    public class UserGetResponse
    {
        /// <summary>
        /// User display name
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// Account creation date
        /// </summary>
        public DateTime CreatedAt { get; set; }
        /// <summary>
        /// Combined ratings of all user posts
        /// </summary>
        public int Reputation { get; set; }
    }
}
