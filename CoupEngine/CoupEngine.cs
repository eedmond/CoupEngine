using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
            Stack<Role> initialCards = ShuffleInitialCards();

            for (int i = 0; i < playerProcessStrings.Length; ++i)
            {
                Role card1 = initialCards.Pop();
                Role card2 = initialCards.Pop();
                PlayerList.Add(new Player(i, card1, card2, playerProcessStrings[i]));
            }

            if (initialCards.Count < 2)
                throw new InvalidOperationException("We don't have enough cards in the pool for Ambassador");
            
            RolePool = new RolePool(initialCards);
        }

        public void PlayGame()
        {
            foreach (var player in PlayerList)
            {
                player.NotifyGameStart(PlayerList.Count);
            }

            while (PlayerList.Count > 1)
            {
                GameAction turn = ActivePlayer.ChooseNextAction(this);
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

        // Converts player id to Player instance. Returns null on failure
        public Player LookupPlayerId(int playerId)
        {
            return PlayerList.Where(p => p.PlayerId == playerId).SingleOrDefault();
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

        private Stack<Role> ShuffleInitialCards()
        {
            // Our deck starts with 3 of each role
            List<Role> deck = new List<Role>(15);
            foreach (Role role in typeof(Role).GetEnumValues())
            {
                deck.Add(role);
                deck.Add(role);
                deck.Add(role);
            }

            // TODO: this is super inefficient, and can easily be made faster if we care
            // Now we shuffle these randomly
            Stack<Role> shuffled = new Stack<Role>(15);
            Random rand = new Random();
            while (deck.Count > 0)
            {
                int index = rand.Next(deck.Count);
                shuffled.Push(deck[index]);
                deck.RemoveAt(index);
            }

            return shuffled;
        }
    }
}
