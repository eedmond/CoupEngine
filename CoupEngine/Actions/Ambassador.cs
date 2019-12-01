using System;
using System.Collections.Generic;
using System.Text;

namespace CoupEngine.Actions
{
    internal class Ambassador : GameAction
    {
        public Ambassador(Player actingPlayer)
            : base(actingPlayer)
        {
        }

        public override bool CanPlayerPerformAction()
        {
            return ActivePlayer.HasRole(Role.Ambassador);
        }

        public override void PerformInternal(CoupEngine engine)
        {
            var role1 = engine.RolePool.DrawRandomRole();
            var role2 = engine.RolePool.DrawRandomRole();

            var returned = ActivePlayer.LookAndReturnToPool(new[] { role1, role2 });
            foreach (var role in returned)
            {
                engine.RolePool.ReturnRole(role);
            }
        }

        public override string SerializeAction()
        {
            var roleStr = ResponseParser.SerializeRole(Role.Ambassador);
            return $"{roleStr} {ActivePlayer.PlayerId}";
        }
    }
}
