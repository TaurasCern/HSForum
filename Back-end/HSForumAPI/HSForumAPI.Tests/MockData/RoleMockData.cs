using HSForumAPI.Domain.Enums;
using HSForumAPI.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSForumAPI.Tests.MockData
{
    public static class RoleMockData
    {
        public static Role[] Data = new Role[3]
        {
            new Role() { RoleId = 1 , Name = ERole.User },
            new Role() { RoleId = 2 , Name = ERole.Admin },
            new Role() { RoleId = 3 , Name = ERole.Moderator }
        };
    }
}
