﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CoupEngine.Turns
{
    internal class ForeignAid : GameAction
    {
        private const int MoneyGained = 2;

        public ForeignAid(Player activePlayer) : base(activePlayer)
        {
        }

        public override bool CanPlayerPerformAction()
        {
            return true;
        }

        public override bool IsBlockedByRole(Role role)
        {
            return role == Role.Duke;
        }

        public override void PerformInternal(CoupEngine engine)
        {
            int moneyGained = Math.Min(engine.MoneyPool, MoneyGained);
            engine.MoneyPool -= moneyGained;
            this.ActivePlayer.Money += moneyGained;
        }

        public override string SerializeAction()
        {
            return $"FA {ActivePlayer.PlayerId}";
        }
    }
}
