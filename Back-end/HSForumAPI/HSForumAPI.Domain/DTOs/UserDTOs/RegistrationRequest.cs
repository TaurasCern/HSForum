namespace HSForumAPI.Domain.DTOs.UserDTOs
{
    public class RegistrationRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}