using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSForumAPI.Domain.DTOs.UserDTOs
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Username is required")]
        [MaxLength(16, ErrorMessage = "Username cannot be longer than 16 characters")]
        [MinLength(3, ErrorMessage = "Username cannot be shorter than 3 characters")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [MaxLength(20, ErrorMessage = "Password cannot be longer than 20 characters")]
        [MinLength(7, ErrorMessage = "Password cannot be shorter than 7 characters")]
        public string Password { get; set; }
    }
}
