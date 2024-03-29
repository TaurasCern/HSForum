﻿using System;
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
        /// Was the existing rating altered
        /// </summary>
        public bool WasAltered { get; set; }
        /// <summary>
        /// Post id of the post that was rated
        /// </summary>
        public int PostId { get; set; }
        /// <summary>
        /// User id of the user that rated the post
        /// </summary>
        public int UserId { get; set; }
    }
}
