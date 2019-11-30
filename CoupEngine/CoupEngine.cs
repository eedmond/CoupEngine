using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CoupEngine
{
    internal class CoupEngine
    {
        public List<Player> PlayerList { get; private set; }
        public RolePool RolePool { get; private set; }

        private int ActivePlayerIndex { get; set; } = 0;
        private Player ActivePlayer { get { return PlayerList[ActivePlayerIndex]; } }
        

        public IEnumerable<Player> OtherPlayers(Player player)
        {
            int playerIndex = GetPlayerIndex(player);

            // Returns all players other than the active player, in turn order
            // starting from the player after the active player
            for (int i = 1; i < PlayerList.Count; ++i)
            {
                int otherPlayerIndex = (playerIndex + i) % PlayerList.Count;
                yield return PlayerList[otherPlayerIndex];
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

        public void EliminatePlayer(Player player)
        {
            player.NotifyEliminated();
            PlayerList.Remove(player);

            foreach (var remainingPlayer in PlayerList)
            {
                remainingPlayer.NotifyPlayerEliminated(player);
            }
        }

        private int GetPlayerIndex(Player player)
        {
            for (int i = 0; i < PlayerList.Count; ++i)
            {
                if (PlayerList[i] == player)
                {
                    return i;
                }
            }

            throw new ArgumentException("No index for player was found");
        }
    }
}
