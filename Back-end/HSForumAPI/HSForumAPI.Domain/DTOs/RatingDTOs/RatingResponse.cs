using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSForumAPI.Domain.DTOs.RatingDTOs
{
    public class RatingResponse
    {
        /// <summary>
        /// Status of the rating that was given
        /// </summary>
        public bool IsPositive { get; set; }
        /// <summary>
        /// Post id of the post that was rated
        /// </summary>
        public int PostId { get; set; }

    }
}
