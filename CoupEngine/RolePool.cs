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

        public List<Role> DrawTwoRandomRoles()
        {
            Debug.Assert(roles.Count == 3);
            int indexKept = random.Next(3);
            var keptRole = roles[indexKept];

            var result = roles;
            result.RemoveAt(indexKept);

            roles = new List<Role> { keptRole };

            return result;
        }

        public void ReturnTwoCards(Role role1, Role role2)
        {
            roles.Add(role1);
            roles.Add(role2);

            Debug.Assert(roles.Count == 3);
        }

        private List<Role> roles;
        private Random random = new Random();
    }
}
