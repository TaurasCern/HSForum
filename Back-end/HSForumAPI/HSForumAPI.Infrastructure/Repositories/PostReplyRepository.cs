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
    public class PostReplyRepository : Repository<PostReply>, IPostReplyRepository
    {
        private readonly HSForumContext _db;

        public PostReplyRepository(HSForumContext db) : base(db)
        {
            _db = db;
        }
        public async Task<PostReply> GetWithUserAsync(Expression<Func<PostReply, bool>> filter, bool tracked = true)
        {
            IQueryable<PostReply> query = _db.PostReplies
                .Include(r => r.User);


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
