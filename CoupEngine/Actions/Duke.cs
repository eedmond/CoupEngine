using System;
using System.Collections.Generic;
using System.Text;

namespace CoupEngine.Actions
{
    internal class Duke : GameAction
    {
        private const int MoneyGained = 3;

        public Duke(Player actingPlayer)
            : base(actingPlayer)
        {
        }

        public override bool CanPlayerPerformAction()
        {
            return ActivePlayer.HasRole(Role.Duke);
        }

        public override void PerformInternal(CoupEngine engine)
        {
            int moneyToGain = Math.Min(MoneyGained, engine.MoneyPool);
            engine.MoneyPool -= moneyToGain;
            ActivePlayer.Money += 3;
        }

        public override string SerializeAction()
        {
            var roleStr = ResponseParser.SerializeRole(Role.Duke);
            return $"{roleStr} {ActivePlayer.PlayerId}";
        }
    }
}
