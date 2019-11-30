using CoupEngine.Turns;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoupEngine
{
    internal class Player
    {
        public void LoseLife()
        {
            throw new NotImplementedException();
        }

        public GameAction ChooseNextAction()
        {
            throw new NotImplementedException();
        }

        public void NotifyAction(GameAction action, Player actingPlayer)
        {
            throw new NotImplementedException();
        }

        public ResponseAction GetResponse()
        {
            throw new NotImplementedException();
        }

        public void NotifyEliminated()
        {
            throw new NotImplementedException();
        }

        public void NotifyPlayerElminated(Player player)
        {
            throw new NotImplementedException();
        }

        public void NotifyWin()
        {
            throw new NotImplementedException();
        }

        public int Money = 0;
    }
}
