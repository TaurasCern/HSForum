using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSForumAPI.Domain.DTOs.RatingDTOs
{
    public class RatingRequest
    {
        /// <summary>
        /// True if rating given by user is positive
        /// False if rating given by user is negative
        /// </summary>
        [Required]
        public bool IsPositive { get; set; }
        /// <summary>
        /// Id of the post that was rated
        /// </summary>
        [Required]
        public int PostId { get; set; }
    }
}
