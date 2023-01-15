using HSForumAPI.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSForumAPI.Infrastructure.DataSeeds
{
    public class PostTypeDataSeed
    {
        public readonly static PostType[] Data = new PostType[5]
        {
            new PostType()
            {
                PostTypeId = 1,
                Type = Domain.Enums.EPostType.News
            },
             new PostType()
            {
                PostTypeId = 2,
                Type = Domain.Enums.EPostType.Tech_help
            },
            new PostType()
            {
                PostTypeId = 3,
                Type = Domain.Enums.EPostType.Intel
            },
            new PostType()
            {
                PostTypeId = 4,
                Type = Domain.Enums.EPostType.AMD
            },
            new PostType()
            {
                PostTypeId = 5,
                Type = Domain.Enums.EPostType.Nvidia
            },
        };
    }
}