using System;
using System.Collections.Generic;
using System.Text;

namespace CoupEngine.Actions
{
    internal class Coup : GameAction
    {
        private const int Cost = 7;

        private Player CoupedPlayer;

        public Coup(Player actingPlayer, Player coupedPlayer)
            : base(actingPlayer)
        {
            CoupedPlayer = coupedPlayer;
            AcceptsResponses = false;
        }

        public override bool CanPlayerPerformAction()
        {
            return true;
        }

        public override void PerformInternal(CoupEngine engine)
        {
            ActivePlayer.Money -= Cost;
            CoupedPlayer.LoseLife(engine);
        }

        public override string SerializeAction()
        {
            return $"COUP {ActivePlayer.PlayerId} -> {CoupedPlayer.PlayerId}";
        }
    }
}
