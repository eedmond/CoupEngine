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

        public void PlayGame()
        {
            while (PlayerList.Count > 1)
            {
                Turn turn = ActivePlayer.GetTurnAction();
                turn.Perform(this);

                ActivePlayerIndex++;
            }
        }
    }
}
