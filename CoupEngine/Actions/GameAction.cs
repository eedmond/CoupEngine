using System;
using System.Collections.Generic;
using System.Text;

namespace CoupEngine
{
    internal abstract class GameAction
    {
        public GameAction(Player activePlayer)
        {
            this.ActivePlayer = activePlayer;
        }

        public void Perform(CoupEngine engine)
        {
            BroadcastAction(engine);
            AskForResponses(engine);

            if (IsValid)
            {
                PerformInternal(engine);
            }
        }

        public abstract void PerformInternal(CoupEngine engine);

        public abstract bool CanPlayerPerformAction();

        public bool IsValid = true;

        protected Player ActivePlayer;
        protected bool AcceptsResponses = true;

        private void BroadcastAction(CoupEngine engine)
        {
            foreach (Player player in engine.OtherPlayers(ActivePlayer))
            {
                player.BroadcastTurn(this, ActivePlayer);
            }
        }

        private void AskForResponses(CoupEngine engine)
        {
            foreach (Player player in engine.OtherPlayers(ActivePlayer))
            {
                var response = player.GetResponse();

                if ((response != null) && this.AcceptsResponses)
                {
                    response.Perform();

                    // If this action has been canceled, break out of response loop
                    if (!this.IsValid)
                    {
                        break;
                    }
                }
            }
        }
    }
}
