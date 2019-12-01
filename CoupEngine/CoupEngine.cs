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
        private const int InvalidPlayerIndex = -1;
        private int NextActivePlayerIndex;
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

        public CoupEngine(string[] playerProcessStrings)
        {
            for (int i = 0; i < playerProcessStrings.Length; ++i)
            {
                throw new NotImplementedException("Deal roles correctly");
                PlayerList.Add(new Player(i, Role.Ambassador, Role.Assassin, playerProcessStrings[i]));
            }
        }

        public void PlayGame()
        {
            while (PlayerList.Count > 1)
            {
                GameAction turn = ActivePlayer.ChooseNextAction();
                turn.Perform(this);

                UpdateActivePlayerIndex();
            }

            PlayerList[0].NotifyWin();
        }

        public void EliminatePlayer(Player player)
        {
            // Update active player index accordingly
            int eliminatedPlayerIndex = GetPlayerIndex(player);

            if (eliminatedPlayerIndex < ActivePlayerIndex)
            {
                --ActivePlayerIndex;
            }
            else if (eliminatedPlayerIndex == ActivePlayerIndex)
            {
                NextActivePlayerIndex = ActivePlayerIndex;
                ActivePlayerIndex = InvalidPlayerIndex;
            }

            // Notify players of elimination
            player.NotifyEliminated();
            PlayerList.Remove(player);

            foreach (var remainingPlayer in PlayerList)
            {
                remainingPlayer.NotifyPlayerEliminated(player);
            }
        }

        private void UpdateActivePlayerIndex()
        {
            // Sets ActivePlayerIndex to the next player.
            if (ActivePlayerIndex != InvalidPlayerIndex)
            {
                // Increment ActivePlayerIndex
                ActivePlayerIndex = (ActivePlayerIndex + 1) % PlayerList.Count;
            }
            else
            {
                // The active player last turn was killed, set the next active player index directly
                ActivePlayerIndex = NextActivePlayerIndex % PlayerList.Count;
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
