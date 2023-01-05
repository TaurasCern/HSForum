using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSForumAPI.Domain.Models
{
    public class LocalUser
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

        public DateTime CreatedAt { get; set; }
        public ICollection<Post> Posts { get; set; }
        public ICollection<PostReply> Replies { get; set; }
        public ICollection<Rating> Ratings { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
    }
}
