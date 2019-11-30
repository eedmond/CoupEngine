using System;
using System.Collections.Generic;
using System.Text;

namespace CoupEngine.Turns
{
    internal class Income : GameAction
    {
        private const int MoneyGained = 3;

        public Income(Player activePlayer) : base(activePlayer)
        {
            AcceptsResponses = false;
        }

        public override bool CanPlayerPerformAction()
        {
            return true;
        }

        public override void PerformInternal(CoupEngine engine)
        {
            int incomeGained = Math.Min(engine.MoneyPool, MoneyGained);
            engine.MoneyPool -= incomeGained;
            this.ActivePlayer.Money += incomeGained;
        }
    }
}
