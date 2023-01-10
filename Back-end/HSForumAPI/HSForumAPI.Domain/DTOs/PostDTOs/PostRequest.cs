using HSForumAPI.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSForumAPI.Domain.DTOs.PostDTOs
{
    public class PostRequest
    {
        /// <summary>
        /// Title of the post
        /// </summary>
        [Required]
        [MinLength(3, ErrorMessage = "Title length has to be at least 3 characters long")]
        [MaxLength(50, ErrorMessage = "Title length cannot be longer than 50 characters")]
        public string Title { get; set; }
        /// <summary>
        /// Contents of the post
        /// </summary>
        [Required]
        [MaxLength(1000, ErrorMessage = "Content cannot be longer 1000 characters")]
        public string Content { get; set; }
        /// <summary>
        /// Enum value of the post
        /// 1. News
        /// </summary>
        [Required]
        [EnumDataType(typeof(EPostType), ErrorMessage = "Cannot parse given string")]
        public string PostType { get; set; }
    }
}
