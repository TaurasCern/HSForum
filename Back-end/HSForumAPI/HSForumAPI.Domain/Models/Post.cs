using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSForumAPI.Domain.Models
{
    public class Post
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }


        public int UserId { get; set; }
        public LocalUser User { get; set; }
        public int PostTypeId { get; set; }
        public PostType PostType { get; set; }
        public ICollection<PostReply> Replies { get; set; }

        public ICollection<Rating> Ratings { get; set; }
    }
}
