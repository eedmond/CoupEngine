using System;
using System.Collections.Generic;
using System.Text;

namespace CoupEngine
{
    internal abstract class GameAction
    {
        public void Perform(CoupEngine engine)
        {
            // Broadcast action to all players, allowing them to cancel if possible
            foreach (Player player in engine.OtherPlayers)
            {
                var response = player.BroadcastTurn(this, engine.ActivePlayer);

                if ((response != null) && this.AcceptsResponses)
                {
                    response.Perform();
                }
            }
        }

        public abstract void PerformInternal(CoupEngine engine);
    }
}
