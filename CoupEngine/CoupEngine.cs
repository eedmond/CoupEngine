using System;
using System.Collections.Generic;
using System.Text;

namespace CoupEngine
{
    class CoupEngine
    {
        TurnManager turnManager;

        public void PlayGame()
        {
            while (turnManager.NumPlayersRemaining > 1)
            {
                TurnActionType turn = turnManager.NextPlayer().ChooseNextAction();

            }
        }
    }
}
