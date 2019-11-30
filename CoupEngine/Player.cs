using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace CoupEngine
{
    internal class Player
    {
        public void LoseLife(CoupEngine engine)
        {
            // TODO: ask the process which role they want to kill, and call LoseLifeInternal
            throw new NotImplementedException();
        }

        private void LoseLifeInternal(CoupEngine engine, bool loseRole1)
        {
            var otherPlayers = engine.OtherPlayers(this);
            // TODO: when a role is lost, reveal it on the table - this is fun because it needs to have access to the engine
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

        public GameAction GetTurnAction()
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

        private Role role1;
        private Role? role2;
    }
}
