using System.ComponentModel.DataAnnotations;

namespace HSForumAPI.Domain.DTOs.PostDTOs
{
    public  class PostUpdateRequest
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
        /// Post creation date
        /// </summary>
        [Required]
        public DateTime CreatedAt { get; set; }
    }
}
