using HSForumAPI.Domain.Enums;

namespace HSForumAPI.Domain.Models
{
    public class PostType
    {
        public int PostTypeId { get; set; }
        public EPostType Type { get; set; }
        public ICollection<Post> Posts { get; set; }
    }
}