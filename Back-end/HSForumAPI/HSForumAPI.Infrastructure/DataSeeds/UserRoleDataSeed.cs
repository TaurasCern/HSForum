using HSForumAPI.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSForumAPI.Infrastructure.DataSeeds
{
    public static class UserRoleDataSeed
    {
        public static readonly UserRole Data = new UserRole() 
        { 
            UserRoleId = 1,
            UserId = 1,
            RoleId = 2, // Admin
        };
    }
}
