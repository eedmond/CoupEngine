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

            if (AcceptsResponses)
            {
                AskForResponses(engine);
            }

            if (IsValid)
            {
                PerformInternal(engine);
            }
        }

        public abstract void PerformInternal(CoupEngine engine);

        public abstract bool CanPlayerPerformAction();

        public abstract string SerializeAction();

        public virtual bool IsBlockedByRole(Role role)
        {
            return false;
        }

        public bool IsValid = true;

        protected Player ActivePlayer;
        protected bool AcceptsResponses = true;

        private void BroadcastAction(CoupEngine engine)
        {
            foreach (Player player in engine.OtherPlayers(ActivePlayer))
            {
                player.NotifyAction(this);
            }
        }

        private void AskForResponses(CoupEngine engine)
        {
            foreach (Player player in engine.OtherPlayers(ActivePlayer))
            {
                var response = player.GetResponse(this);

                if (response != null)
                {
                    response.Perform(engine);

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
