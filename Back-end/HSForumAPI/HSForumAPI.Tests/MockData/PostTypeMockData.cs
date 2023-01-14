using HSForumAPI.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSForumAPI.Tests.MockData
{
    public class PostTypeMockData
    {
        public readonly static PostType[] Data = new PostType[1]
        {
            new PostType()
            {
                PostTypeId = 1,
                Type = Domain.Enums.EPostType.News
            },
        };
    }
}
