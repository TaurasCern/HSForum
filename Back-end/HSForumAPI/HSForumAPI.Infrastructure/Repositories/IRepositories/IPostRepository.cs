using HSForumAPI.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HSForumAPI.Infrastructure.Repositories.IRepositories
{
    public interface IPostRepository : IRepository<Post>
    {
        Task<Post> GetWithRepliesAsync(Expression<Func<Post, bool>> filter, bool tracked = true);
    }
}
