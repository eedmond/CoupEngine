using System;
using System.Collections.Generic;
using System.Text;

namespace CoupEngine.Turns
{
    internal abstract class ResponseAction : GameAction
    {
        protected GameAction OriginalAction;

        public ResponseAction(GameAction originalAction, Player actingPlayer) : base(actingPlayer)
        {
            OriginalAction = originalAction;
        }
    }
}
