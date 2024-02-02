using CardRPG.Entities.Users;
using System.Collections.Generic;
using System.Linq;

namespace CardRPG.Entities.Gameplay
{
    public static class PlayerExtensions
    {
        public static Player OfId(this IEnumerable<Player> players, UserId id) =>
            players.First(p => p.Id == id);

        public static Player NotOfId(this IEnumerable<Player> players, UserId id) =>
            players.First(p => p.Id != id);

        public static UserId[] Ids(this IEnumerable<Player> players) =>
            players.Select(c => c.Id).ToArray();
    }
}
