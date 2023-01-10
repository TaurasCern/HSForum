using System.ComponentModel.DataAnnotations;

namespace HSForumAPI.Domain.DTOs.UserDTOs
{
    public class RegistrationRequest
    {
        [Required(ErrorMessage = "Username is required")]
        [MaxLength(16, ErrorMessage = "Username cannot be longer than 16 characters")]
        [MinLength(3, ErrorMessage = "Username cannot be shorter than 3 characters")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Username is required")]
        [MaxLength(20, ErrorMessage = "Password cannot be longer than 20 characters")]
        [MinLength(7, ErrorMessage = "Password cannot be shorter than 7 characters")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; }
    }
}