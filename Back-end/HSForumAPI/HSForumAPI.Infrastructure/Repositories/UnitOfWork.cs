using HSForumAPI.Infrastructure.Database;
using HSForumAPI.Infrastructure.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSForumAPI.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly HSForumContext _db;
        public IUserRepository Users { get; private set; }
        public IUserRoleRepository UserRoles { get; private set; }
        public IPostRepository Posts { get; private set; }
        public IPostReplyRepository PostReplies { get; private set; }
        public IRatingRepository Ratings { get; private set; }
        public IPostTypeRepository PostTypes { get; private set; }

        public UnitOfWork(HSForumContext db, 
            IUserRepository userRepository,
            IUserRoleRepository userRoleRepository,
            IPostRepository posts,
            IPostReplyRepository postReplies,
            IRatingRepository ratings,
            IPostTypeRepository postTypes)
        {
            _db = db;
            Users = userRepository;
            UserRoles = userRoleRepository;
            Posts = posts;
            PostReplies = postReplies;
            Ratings = ratings;
            PostTypes = postTypes;
        }
        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
