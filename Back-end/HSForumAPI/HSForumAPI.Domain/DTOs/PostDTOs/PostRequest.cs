using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSForumAPI.Domain.DTOs.PostDTOs
{
    public class PostRequest
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string PostType { get; set; }
    }
}
