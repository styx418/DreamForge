namespace DreamCraftServer0._02.Entities
{
    public static class PlayerManager
    {
        private static readonly Dictionary<int, Player> Players = new();

        public static bool AddPlayer(Player player)
        {
            if (Players.ContainsKey(player.Id))
                return false;

            Players[player.Id] = player;
            return true;
        }

        public static void DisconnectAll()
        {
            foreach (var player in Players.Values)
            {
                player.Disconnect("Déconnexion globale");
            }

            Players.Clear();
        }

        public static void SaveAll()
        {
            foreach (var player in Players.Values)
            {
                player.Save();
            }
        }

        public static IEnumerable<Player> GetAllPlayers() => Players.Values;

        public static bool RemovePlayer(int id) => Players.Remove(id);

        public static Player GetPlayerById(int id)
            => Players.TryGetValue(id, out var player) ? player : null;
    }
}
