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
