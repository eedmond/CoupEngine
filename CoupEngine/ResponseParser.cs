using CoupEngine.Turns;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoupEngine
{
    internal class ResponseParser
    {
        public GameAction ParseAction(string str, Player player)
        {
            throw new NotImplementedException();
        }

        public ResponseAction ParseResponse(string str, Player player)
        {
            throw new NotImplementedException();
        }

        public string SerializeRole(Role role)
        {
            throw new NotImplementedException();
        }

        public IList<Role> ParseAmbassador(string str)
        {
            throw new NotImplementedException();
        }

        public Role? ParseRoleLost(string str)
        {
            throw new NotImplementedException();
        }
    }
}
