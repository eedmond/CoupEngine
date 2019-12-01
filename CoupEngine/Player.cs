using CoupEngine.Turns;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace CoupEngine
{
    // Full list of our protocol:
    // Sending: Every time we send something, the first word designates what happened,
    // then comes any details (who played it, etc), finally ended by a newline.
    // If we are expecting a response, the last thing before the newline should be a '?' character
    // 
    // Note that all requests/responses should be considered case-insensitive: the process doesn't have to care about receiving/outputing 'AM' vs 'am' vs 'Am'
    //
    // Serialization for roles:
    //   AM : ambassador
    //   AS : assassin
    //   CAP : captain
    //   CON : contessa
    //   DUKE: duke
    // 
    // Notifications that an action was taken:
    // Whenever a player takes an action, all *other* players are notified with a message. (The player itself should obviously know what happened)
    // If an action is obviously associated with a role, then it will look like this:
    //    <role> <acting player> ['->' <targeted player>]
    // For example:
    //    DUKE 2
    //    CAP 3 -> 1
    //    AS 1 -> 2
    //
    // For actions that don't have an associated role, it will be the following:
    //   IN <actingplayer> : Income
    //   FA <actingplayer> : Foreign Aid
    //   CH <actingplayer> : Challenge
    //   CLAIM <actingplayer> <role> : Role claim (for blocking another action)
    // 

    // Serialization for requests:
    // Whenever the engine wants a response from a player, one of these will be sent:
    //   R? : request for a 'response' to an action another player took (challenge or claim)
    //   L? : losing a life, request which card they want to reveal (this doesn't happen if they only have one choice)
    //         Expected response: L <role>
    //   A? : request for what action they want to take (they are the active player now)
    //   AM? <role> <role> : here are the cards drawn by the Ambassador action, request for what they want to discard
    //                       Expected response: AM <role> <role>
    //
    // Serialization for responses:
    // Responses for actions (the 'A?' or 'R?' request) should be similar to above actions, but no 'actingplayer' id is necessary.
    // In general this should look like:
    //    <role/action> [<target player>]
    // For example:
    //    IN
    //    AS 2
    //    CLAIM CON

    // Other notifications:
    // - GLHF <id> <numplayers> <role> <role>: Game start notification, let's the process know which id it is, # players, starting cards
    //                                          (Note that player id 0 always goes first)
    // - L <player> <role> : The given player lost a life, and revealed this role
    // - RIP <player> : The given player was eliminated
    // - EL : You were eliminated :(
    // - GG : You won!

    // TODO?: change the Ambassador protocol to be more generic (2 draw notifications and 2 discard notifications)

    internal class Player
    {
        public Player(int id, Role role1, Role role2, string processString)
        {
            PlayerId = id;
            this.role1 = role1;
            this.role2 = role2;
            process = new PlayerProcess(processString);
        }

        public void LoseLife(CoupEngine engine)
        {
            // TODO: decide which card to lose, and call LoseLifeInternal with it
            throw new NotImplementedException();
        }

        public GameAction ChooseNextAction()
        {
            process.SendMessage("A?");
            string response = process.ReceiveResponse();
            GameAction action = parser.ParseAction(response, this);
            
            if (action == null) // invalid response
            {
                // TODO: log warning
                return new Income(this);
            }
            
            // TODO: validate / execute
            // - money cost
            // - targeted player exists at all
            // - targeted player hasn't been eliminated

            return action;
        }

        public void NotifyAction(GameAction action)
        {
            process.SendMessage(action.SerializeAction());
        }

        public ResponseAction GetResponse()
        {
            process.SendMessage("R?");
            string response = process.ReceiveResponse();
            ResponseAction action = parser.ParseResponse(response, this);

            if (action == null) // invalid response
            {
                // TODO: log warning
                return null; // Don't do anything
            }

            // TODO: validate
            // - The action can be blocked (i.e. Ambassador can be challenged, but not blocked)
            // - The action can be challenged (i.e. Foreign Aid can be blocked, but not challenged)
            // - A blocking claim is even valid (i.e. Duke doesn't block Assassin)

            return action;
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

        public IEnumerable<Role> LookAndReturnToPool(IList<Role> drawnCards)
        {
            var roleStrings = drawnCards.Select(r => parser.SerializeRole(r));
            var allRoles = String.Join(' ', roleStrings);

            process.SendMessage($"AM? {allRoles}");
            string response = process.ReceiveResponse();

            var rolesToReturn = parser.ParseAmbassador(response);
            if (rolesToReturn == null) // Invalid response
            {
                // TODO: log warning
                // Don't make any changes to the player's hand
                return drawnCards; 
            }

            // TODO: validate
            // - the returned cards are ones that were drawn/owned

            // TODO: implement - keep the cards that weren't returned
            throw new NotImplementedException();
        }

        #region Notify Messages

        public void NotifyEliminated()
        {
            role2 = null;
            Eliminated = true;

            // Tell the process that we lost :(
            process.SendMessage("EL");
        }

        public void NotifyPlayerEliminated(Player player)
        {
            process.SendMessage($"RIP {player.PlayerId}");
        }

        public void NotifyWin()
        {
            process.SendMessage("GG");
        }

        public void NotifyLifeLost(Player player, Role role)
        {
            // Tell the process that 'player' lost a life and revealed the card 'role'
            process.SendMessage($"L {player.PlayerId} {parser.SerializeRole(role)}");
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

        private PlayerProcess process;
        private ResponseParser parser;
    }
}
