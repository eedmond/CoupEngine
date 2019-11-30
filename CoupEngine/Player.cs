using CoupEngine.Turns;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace CoupEngine
{
    // Full list of our protocol:
    // Sending: Every time we send something, the first word designates what happened,
    // then comes any details (who played it, etc), finally ended by a newline.
    // If we are expecting a response, the last thing before the newline should be a '?' character
    // 
    // Serialization for our actions (details excluded)
    // - AM : ambassador
    // - AS : assassinate
    // - CH! : challenge
    // - COUP : coup
    // - FA : Foreign Aid
    // - IN : Income
    // - CLAIM : Role claim
    // - CAP : captain / steal
    //
    // Serialization for requests:
    // - R? : request for a 'response' to an action another player took (challenge or claim)
    // - L? : losing a life, request which card they want to reveal (this doesn't happen if they only have one card)
    // - A? : request for what action they want to take (they are the active player now)
    //
    // Serialization for reponses should be either a serialization for one of the actions above. If the action targets a player,
    // just put the player number ('PlayerId') as a second word.
    // For the response to 'L?' it should be the abbreviated name of a role, which are listed as above, or in addition:
    // - CONT : Contessa
    // - DUKE : Duke
    //
    // Note that all requests/responses should be considered case-insensitive: the process doesn't have to care about receiving/outputing 'AM' vs 'am' vs 'Am'

    internal class Player
    {
        public void LoseLife(CoupEngine engine)
        {
            // TODO: decide which card to lose, and call LoseLifeInternal with it
            throw new NotImplementedException();
        }

        public GameAction ChooseNextAction()
        {
            throw new NotImplementedException();
        }

        public void NotifyAction(GameAction action, Player actingPlayer)
        {
            // TODO: actually send string
            string toSend = action.SerializeAction();
        }

        public ResponseAction GetResponse()
        {
            // TODO: ask the process which role they want to kill, and call LoseLifeInternal
            throw new NotImplementedException();
        }

        private void LoseLifeInternal(CoupEngine engine, bool loseRole1)
        {
            var otherPlayers = engine.OtherPlayers(this);
            if (loseRole1)
            {
                // Tell the other players what the role we lost was
                foreach (var player in otherPlayers)
                {
                    player.NotifyLifeLost(this, role1);
                }

                if (role2.HasValue)
                {
                    role1 = role2.Value;
                    role2 = null;
                }
                else
                {
                    // This was their only role card, eliminate the player
                    engine.EliminatePlayer(this);
                }
            }
            else
            {
                Debug.Assert(role2.HasValue);

                // Tell the other players what the role we lost was
                foreach (var player in otherPlayers)
                {
                    player.NotifyLifeLost(this, role2.Value);
                }

                role2 = null;
            }
        }

        public bool HasRole(Role role)
        {
            if (Eliminated)
                return false;

            if (role2.HasValue && role2.Value == role)
                return true;

            return role1 == role;
        }

        public IEnumerable<Role> LookAndReturnToPool(IEnumerable<Role> drawnCards)
        {
            throw new NotImplementedException();
        }

        #region Notify Stubs

        public void NotifyEliminated()
        {
            role2 = null;
            Eliminated = true;

            // TODO: tell the process that we lost :(
            throw new NotImplementedException();
        }

        public void NotifyPlayerEliminated(Player player)
        {
            // TODO: tell the process that 'player' 
            throw new NotImplementedException();
        }

        public void NotifyWin()
        {
            // TODO: tell the process that they won!
            throw new NotImplementedException();
        }

        public void NotifyLifeLost(Player player, Role role)
        {
            // TODO: tell the process that 'player' lost a life and revealed the card 'role'
            throw new NotImplementedException();
        }

        #endregion

        public int Money = 3;

        public bool Eliminated { get; private set; } = false;
        public int NumRoles
        {
            get
            {
                if (role2.HasValue)
                    return 2;

                if (Eliminated)
                    return 0;
                return 1;
            }
        }

        public int PlayerId { get; }

        private Role role1;
        private Role? role2;
    }
}
