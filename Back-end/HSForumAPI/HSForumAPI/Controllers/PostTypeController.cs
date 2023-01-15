using HSForumAPI.Domain.DTOs.PostTypeDTOs;
using HSForumAPI.Domain.Services.IServices;
using HSForumAPI.Infrastructure.Repositories.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HSForumAPI.Controllers
{
    /// <summary>
    /// Controller to handle PostType endpoints
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PostTypeController : ControllerBase
    {
        private readonly IUnitOfWork _db;
        private readonly IAdapterService _adapter;
        public PostTypeController(
            IUnitOfWork db,
            IAdapterService adapter)
        {
            _db = db;
            _adapter = adapter;
        }
        /// <summary>
        /// Fetches all post types from the database
        /// </summary>
        /// <returns>All post types</returns>
        /// <remarks>
        /// Sample:    
        ///     GET PostType
        /// </remarks>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(PostTypeResponse))]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAll()
        {
            var response = (await _db.PostTypes.GetAllAsync()).Select(pt => _adapter.Bind(pt));

            return Ok(response);
        }
    }
}
