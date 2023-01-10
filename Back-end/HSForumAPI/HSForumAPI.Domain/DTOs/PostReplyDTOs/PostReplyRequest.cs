using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSForumAPI.Domain.DTOs.PostReplyDTOs
{
    public class PostReplyRequest
    {
        /// <summary>
        /// Contents of the comment
        /// </summary>
        [Required]
        [MinLength(3, ErrorMessage = "Content length has to be at least 3 characters long")]
        [MaxLength(200, ErrorMessage = "Content length cannot be longer than 200 characters")]
        public string Content { get; set; }
        /// <summary>
        /// Id of the post where the comment is made
        /// </summary>
        [Required]
        public int PostId { get; set; }
    }
}
