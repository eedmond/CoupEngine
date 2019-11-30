using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CoupEngine
{
    internal class CoupEngine
    {
        public List<Player> PlayerList { get; private set; }
        public int ActivePlayerIndex { get; private set; } = 0;
        public Player ActivePlayer { get { return PlayerList[ActivePlayerIndex]; } }
        public IEnumerable<Player> OtherPlayers
        {
            get
            {
                // Returns all players other than the active player, in turn order
                // starting from the player after the active player
                for (int i = 1; i < PlayerList.Count; ++i)
                {
                    int playerIndex = (ActivePlayerIndex + i) % PlayerList.Count;
                    yield return PlayerList[playerIndex];
                }
            }
        }

        public int MoneyPool = 30;

        public void PlayGame()
        {
            while (PlayerList.Count > 1)
            {
                GameAction turn = ActivePlayer.ChooseNextAction();
                turn.Perform(this);

                ActivePlayerIndex = (ActivePlayerIndex + 1) % PlayerList.Count;
            }

            PlayerList[0].NotifyWin();
        }

        public RemovePlayer(Player player)
        {
            player.NotifyEliminated();
            PlayerList.Remove(player);

            foreach (var remainingPlayer in PlayerList)
            {
                remainingPlayer.NotifyPlayerEliminated(player);
            }
        }
    }
}
