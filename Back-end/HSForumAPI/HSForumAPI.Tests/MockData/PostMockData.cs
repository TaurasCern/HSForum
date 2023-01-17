using HSForumAPI.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSForumAPI.Tests.MockData
{
    public class PostMockData
    {
        public static Post[] Data = new Post[3]
        {
            new Post() { PostId = 1, IsActive = true, UserId = 1, PostTypeId = 1},
            new Post() { PostId = 1, IsActive = true, UserId = 1, PostTypeId = 1},
            new Post() { PostId = 1, IsActive = true, UserId = 1, PostTypeId = 1}
        };
    }
}
