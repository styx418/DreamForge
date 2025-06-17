using System.Net.Sockets;

namespace DreamCraftServer0._02.Network
{
    public class TcpClientState
    {
        public int Id { get; }
        public TcpClient Client { get; }
        public byte[] Buffer { get; }

        public TcpClientState(int id, TcpClient client)
        {
            Id = id;
            Client = client;
            Buffer = new byte[4096];
        }
    }
}
