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
        public async Task<Post> GetWithRepliesAsync(Expression<Func<Post, bool>> filter, bool tracked = true)
        {
            IQueryable<Post> query = _db.Posts
                .Include(p => p.Replies);
            query = query
                .Include(p => p.Ratings);


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
        public async Task<List<Post>> GetAllWithRatingsAsync(Expression<Func<Post, bool>> filter, bool tracked = true)
        {
            IQueryable<Post> query = _db.Posts
                .Include(p => p.Ratings);

            if (!tracked)
            {
                query = query.AsNoTracking();
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return await query.ToListAsync();
        }
        public override async Task<Post> UpdateAsync(Post post)
        {
            post.UpdatedAt = DateTime.Now;

            _db.Posts.Update(post);
            await _db.SaveChangesAsync();

            return post;
        }
    }
}
