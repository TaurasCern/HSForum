using HSForumAPI.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSForumAPI.Domain.Models
{
    public class Role
    {
        public int RoleId { get; set; }
        public ERole Name { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
    }
}