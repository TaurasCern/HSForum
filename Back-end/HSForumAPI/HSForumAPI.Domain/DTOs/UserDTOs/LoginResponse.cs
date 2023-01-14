namespace HSForumAPI.Domain.DTOs.UserDTOs
{
    public class LoginResponse
    {
        public int UserId { get; set; }
        public string[] Roles { get; set; }
        public string Token { get; set; }
    }
}