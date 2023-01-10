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
    public class RatingRepository : Repository<Rating>, IRatingRepository
    {
        private readonly HSForumContext _db;

        public RatingRepository(HSForumContext db) : base(db)
        {
            _db = db;
        }

        public async Task<Rating> UpdateAsync(Rating rating)
        {
            _db.Ratings.Update(rating);
            await _db.SaveChangesAsync();
            return rating;
        }
    }
}
