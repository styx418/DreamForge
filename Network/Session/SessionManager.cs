using System.Collections.Generic;
using System.Net.Sockets;

namespace DreamCraftServer0._02.Network.Session
{
    public static class SessionManager
    {
        private static readonly Dictionary<TcpClient, ClientSession> _sessions = new();

        public static void Register(TcpClient client)
        {
            if (!_sessions.ContainsKey(client))
                _sessions[client] = new ClientSession(client);
        }

        public static void Unregister(TcpClient client)
        {
            if (_sessions.ContainsKey(client))
                _sessions.Remove(client);
        }

        public static ClientSession Get(TcpClient client)
        {
            return _sessions.TryGetValue(client, out var session) ? session : null;
        }

        public static bool Exists(TcpClient client) => _sessions.ContainsKey(client);
    }
}
