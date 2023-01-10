using HSForumAPI.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSForumAPI.Domain.DTOs.PostDTOs
{
    public class PostGetTypeRequest
    {
        /// <summary>
        /// Enum type of the post
        /// </summary>
        [Required]
        [EnumDataType(typeof(EPostType), ErrorMessage = "Cannot parse given string, no such Enum")]
        public string PostType { get; set; }
    }
}
