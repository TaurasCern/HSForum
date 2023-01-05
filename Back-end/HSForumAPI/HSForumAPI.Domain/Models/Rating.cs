using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSForumAPI.Domain.Models
{
    public class Rating
    {
        public int RatingId { get; set; }
        public bool IsPositive { get; set; }


        public int UserId { get; set; }
        public LocalUser User { get; set; }

        public int PostId { get; set; }
        public Post Post { get; set; }
    }
}
