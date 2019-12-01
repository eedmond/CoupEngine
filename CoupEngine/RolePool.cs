using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace CoupEngine
{
    internal class RolePool
    {
        public RolePool(IEnumerable<Role> initialRoles)
        {
            roles = initialRoles.ToList();
        }

        public Role DrawRandomRole()
        {
            int index = random.Next(roles.Count);
            var result = roles[index];
            roles.RemoveAt(index);
            return result;
        }

        public void ReturnRole(Role role)
        {
            roles.Add(role);
        }

        private List<Role> roles;
        private Random random = new Random();
    }
}
