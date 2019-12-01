using System;
using System.Collections.Generic;
using System.Text;

namespace CoupEngine.Actions
{
    internal class Assassinate : GameAction
    {
        private Player AssassinatedPlayer;

        public Assassinate(Player actingPlayer, Player assassinatedPlayer)
            : base(actingPlayer)
        {
            AssassinatedPlayer = assassinatedPlayer;
        }

        public override bool CanPlayerPerformAction()
        {
            return ActivePlayer.HasRole(Role.Assassin);
        }

        public override bool IsBlockedByRole(Role role)
        {
            return role == Role.Contessa;
        }

        public override void PerformInternal(CoupEngine engine)
        {
            AssassinatedPlayer.LoseLife(engine);
        }

        public override string SerializeAction()
        {
            var roleStr = ResponseParser.SerializeRole(Role.Assassin);
            return $"{roleStr} {ActivePlayer.PlayerId} -> {AssassinatedPlayer.PlayerId}";
        }
    }
}
