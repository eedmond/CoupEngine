using CoupEngine.Actions;
using CoupEngine.Turns;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoupEngine
{
    internal class ResponseParser
    {
        private static Dictionary<Role, string> serializeRoleTable = new Dictionary<Role, string>()
        {
            { Role.Ambassador, "AM" },
            { Role.Assassin, "AS" },
            { Role.Captain, "CAP" },
            { Role.Contessa, "CON" },
            { Role.Duke, "DUKE" },
        };

        private static Dictionary<string, Role> deserializeRoleTable = new Dictionary<string, Role>()
        {
            { "AM", Role.Ambassador },
            { "AS", Role.Assassin },
            { "CAP", Role.Captain },
            { "CON", Role.Contessa },
            { "DUKE", Role.Duke },
        };

        public GameAction ParseAction(string str, Player player, CoupEngine engine)
        {
            if (String.IsNullOrWhiteSpace(str))
                return null;

            str = str.Trim().ToUpper();

            var words = str.Split();
            if (words.Length == 0)
                return null;

            // Grab the first word, and handle it
            switch (words[0])
            {
                // Role base actions
                case "AM":
                    return new Ambassador(player);
                case "AS":
                    return ParseTargetedAction(words, engine, (target) => new Assassinate(player, target));
                case "CAP":
                    return ParseTargetedAction(words, engine, (target) => new Steal(player, target));
                case "DUKE":
                    return new Duke(player);

                // Always available actions
                case "IN":
                    return new Income(player);
                case "FA":
                    return new ForeignAid(player);
                case "COUP":
                    return ParseTargetedAction(words, engine, (target) => new Coup(player, target));
                default:
                    return null;
            }
        }

        private GameAction ParseTargetedAction(string[] words, CoupEngine engine, Func<Player, GameAction> actionFactory)
        {
            if (words.Length != 2)
                return null;

            if (!Int32.TryParse(words[1], out int targetPlayerId))
                return null;

            Player target = engine.LookupPlayerId(targetPlayerId);
            if (target == null)
                return null;

            return actionFactory(target);
        }

        public ResponseAction ParseResponse(string str, Player player, GameAction originalAction)
        {
            if (String.IsNullOrWhiteSpace(str))
                return null;

            str = str.Trim().ToUpper();

            if (str == "N")
            {
                return null;
            }

            if (str == "CH")
            {
                return new Challenge(originalAction, player);
            }

            if (!str.StartsWith("CLAIM"))
                return null; // Invalid

            // We have a "CLAIM", grab the role that's being claimed
            var roleStr = str.Substring("CLAIM".Length).Trim();

            var role = DeserializeRole(roleStr);
            if (role == null)
                return null; // Invalid role

            return new RoleClaim(originalAction, player, role.Value);
        }

        public static string SerializeRole(Role role)
        {
            return serializeRoleTable.GetValueOrDefault(role, null);
        }

        public IList<Role> ParseAmbassador(string str)
        {
            if (String.IsNullOrWhiteSpace(str))
                return null;

            str = str.Trim().ToUpper();

            if (!str.StartsWith("AM"))
                return null;

            string[] rolesDiscardedStrs = str.Substring("AM".Length).Trim().Split(' ');

            // We expect exactly 2 roles here
            if (rolesDiscardedStrs.Length != 2)
                return null;

            var role1 = DeserializeRole(rolesDiscardedStrs[0]);
            var role2 = DeserializeRole(rolesDiscardedStrs[1]);

            if (role1 == null || role2 == null)
                return null;

            return new[] { role1.Value, role2.Value };
        }

        public Role? ParseRoleLost(string str)
        {
            if (String.IsNullOrWhiteSpace(str))
                return null;

            str = str.Trim().ToUpper();

            //if (!str.StartsWith("L"))
            //    return null;

            //var roleStr = str.Substring("L".Length).Trim();
            //return DeserializeRole(roleStr);

            return DeserializeRole(str);
        }

        private static Role? DeserializeRole(string str)
        {
            if (!deserializeRoleTable.TryGetValue(str, out Role role))
                return null;
            return role;
        }

    }
}
