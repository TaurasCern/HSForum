using HSForumAPI.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSForumAPI.Tests.MockData
{
    public static class RatingsMockData
    {
        public static Rating[] Data = new Rating[3]
        {
            new Rating() { RatingId = 1, UserId = 1, PostId = 1, },
            new Rating() { RatingId = 2, UserId = 2, PostId = 1, },
            new Rating() { RatingId = 3, UserId = 3, PostId = 1, }
        };
    }
}
