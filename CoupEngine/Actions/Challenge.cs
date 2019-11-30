using System;
using System.Collections.Generic;
using System.Text;

namespace CoupEngine.Turns
{
    internal class Challenge : ResponseAction
    {
        public Challenge(GameAction originalAction, Player actingPlayer) : base(originalAction, actingPlayer)
        {
            AcceptsResponses = false;
        }

        public override bool CanPlayerPerformAction()
        {
            return true;
        }

        public override void PerformInternal(CoupEngine engine)
        {
            if (!OriginalAction.CanPlayerPerformAction())
            {
                OriginalAction.IsValid = false;
            }
        }
    }
}
