using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace CoupEngine
{
    internal class RolePool
    {
        public RolePool(Role role1, Role role2, Role role3)
        {
            roles = new List<Role> { role1, role2, role3 };
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
