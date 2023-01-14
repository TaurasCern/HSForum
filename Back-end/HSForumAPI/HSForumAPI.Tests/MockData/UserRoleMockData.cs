using HSForumAPI.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSForumAPI.Tests.MockData
{
    public static class UserRoleMockData
    {
        public static readonly List<UserRole> Data = new List<UserRole>() 
        { 
            new UserRole()
            {
                UserRoleId = 1,
                UserId = 1,
                RoleId = 2, // Admin
            }
        };
    }
}
