using HSForumAPI.Domain.Models;
using HSForumAPI.Infrastructure.Database;
using HSForumAPI.Infrastructure.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSForumAPI.Infrastructure.Repositories
{
    public class PostRepository : Repository<Post>, IPostRepository
    {
        private readonly HSForumContext _db;

        public PostRepository(HSForumContext db) : base(db)
        {
            _db = db;
        }
    }
}
