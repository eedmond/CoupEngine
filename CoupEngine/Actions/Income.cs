using System;
using System.Collections.Generic;
using System.Text;

namespace CoupEngine.Turns
{
    internal class Income : GameAction
    {
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
            int incomeGained = Math.Min(engine.MoneyPool, 2);
            engine.MoneyPool -= incomeGained;
            this.ActivePlayer.Money += incomeGained;
        }
    }
}
