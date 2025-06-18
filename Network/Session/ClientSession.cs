using System.Net.Sockets;

namespace DreamCraftServer0._02.Network.Session
{
    public class ClientSession
    {
        public TcpClient Client { get; }
        public int AccountId { get; set; }
        public string Username { get; set; }

        public ClientSession(TcpClient client)
        {
            Client = client;
        }
    }
}
