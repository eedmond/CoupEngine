using System;
using System.Collections.Generic;
using System.Text;

namespace CoupEngine.Turns
{
    internal class RoleClaim : ResponseAction
    {
        public RoleClaim(GameAction originalAction, Player actingPlayer, Role claimedRole) :
            base(originalAction, actingPlayer)
        {
        }

        public override bool CanPlayerPerformAction()
        {
            return ActivePlayer.HasRole(claimedRole);
        }

        public override void PerformInternal(CoupEngine engine)
        {
            throw new NotImplementedException();
        }
    }
}
