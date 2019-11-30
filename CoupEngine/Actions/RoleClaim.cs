using System;
using System.Collections.Generic;
using System.Text;

namespace CoupEngine.Turns
{
    internal class RoleClaim : ResponseAction
    {
        private Role ClaimedRole;

        public RoleClaim(GameAction originalAction, Player actingPlayer, Role claimedRole) :
            base(originalAction, actingPlayer)
        {
            ClaimedRole = claimedRole;
        }

        public override bool CanPlayerPerformAction()
        {
            return ActivePlayer.HasRole(ClaimedRole);
        }

        public override void PerformInternal(CoupEngine engine)
        {
            OriginalAction.IsValid = false;
        }
    }
}
