﻿using HSForumAPI.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSForumAPI.Infrastructure.Repositories.IRepositories
{
    public interface IRatingRepository : IRepository<Rating>
    {
        Task<Rating> UpdateAsync(Rating rating);
    }
}
