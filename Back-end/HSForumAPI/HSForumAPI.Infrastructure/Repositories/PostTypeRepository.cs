using HSForumAPI.Domain.Models;
using HSForumAPI.Infrastructure.Database;
using HSForumAPI.Infrastructure.Repositories.IRepositories;

namespace HSForumAPI.Infrastructure.Repositories
{
    public class PostTypeRepository : Repository<PostType>, IPostTypeRepository
    {
        private readonly HSForumContext _db;
        public PostTypeRepository(HSForumContext db)
            : base(db)
        {
            _db = db;
        }
    }
}
