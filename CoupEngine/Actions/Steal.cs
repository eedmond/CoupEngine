using System;
using System.Collections.Generic;
using System.Text;

namespace CoupEngine.Actions
{
    internal class Steal : GameAction
    {
        private const int StealAmount = 2;

        private Player PlayerToStealFrom;

        public Steal(Player actingPlayer, Player playerToStealFrom)
            : base(actingPlayer)
        {
            PlayerToStealFrom = playerToStealFrom;
        }

        public override bool CanPlayerPerformAction()
        {
            return ActivePlayer.HasRole(Role.Captain);
        }

        public override bool IsBlockedByRole(Role role)
        {
            return (role == Role.Ambassador) || (role == Role.Captain);
        }

        public override void PerformInternal(CoupEngine engine)
        {
            int amountStolen = Math.Min(PlayerToStealFrom.Money, StealAmount);
            PlayerToStealFrom.Money -= amountStolen;
            ActivePlayer.Money += amountStolen;
        }

        public override string SerializeAction()
        {
            var roleStr = ResponseParser.SerializeRole(Role.Captain);
            return $"{roleStr} {ActivePlayer.PlayerId}";
        }
    }
}
