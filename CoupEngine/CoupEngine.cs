using System;
using System.Collections.Generic;
using System.Text;

namespace CoupEngine
{
    internal class CoupEngine
    {
        public List<Player> PlayerList { get; private set; }
        public int ActivePlayerIndex { get; private set; } = 0;
        public Player ActivePlayer { get { return PlayerList[ActivePlayerIndex]; } }

        private Dictionary<TurnActionType, Turn> turnLookup;

        public void PlayGame()
        {
            while (PlayerList.Count > 1)
            {
                TurnActionType turnType = ActivePlayer.GetTurnAction();

                Turn turn = turnLookup[turnType];
                turn.Perform(this);
            }
        }
    }
}
