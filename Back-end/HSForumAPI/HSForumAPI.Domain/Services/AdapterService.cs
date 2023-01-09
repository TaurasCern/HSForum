using HSForumAPI.Domain.DTOs.PostDTOs;
using HSForumAPI.Domain.DTOs.UserDTOs;
using HSForumAPI.Domain.Enums;
using HSForumAPI.Domain.Models;
using HSForumAPI.Domain.Services.IServices;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSForumAPI.Domain.Services
{
    public class AdapterService : IAdapterService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AdapterService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Post Bind(PostRequest request) => new()
        {
            Title = request.Title,
            Content = request.Content,
            CreatedAt = DateTime.Now,
            PostTypeId = (int)(EPostType)Enum.Parse(typeof(EPostType), request.PostType),
            UserId = int.Parse(_httpContextAccessor.HttpContext.User.Identity.Name)
        };
    }
}
