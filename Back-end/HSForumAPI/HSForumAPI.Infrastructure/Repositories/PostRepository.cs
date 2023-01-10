using HSForumAPI.Domain.Models;
using HSForumAPI.Infrastructure.Database;
using HSForumAPI.Infrastructure.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
        public async Task<Post> GetWithReplies(Expression<Func<Post, bool>> filter, bool tracked = true)
        {
            IQueryable<Post> query = _db.Posts
                .Include(p => p.Replies);

            if (!tracked)
            {
                query = query.AsNoTracking();
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return await query.FirstOrDefaultAsync();
        }
    }
}
