using System;
using System.Collections.Generic;
using System.Text;

namespace CoupEngine.Turns
{
    internal class Income : Turn
    {
        public override void Perform(CoupEngine engine)
        {
            int incomeGained = Math.Min(engine.MoneyPool, 2);
            engine.MoneyPool -= incomeGained;
            engine.ActivePlayer.Money += incomeGained;
        }
    }
}
