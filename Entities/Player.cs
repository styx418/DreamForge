using System;
using DreamCraftServer0._02.Data.Models;
using System.Net.Sockets;
using DreamCraftServer0._02.Utils;

namespace DreamCraftServer0._02.Entities
{
    public class Player
    {
        public int Id => Character.CharacterId;
        public string Name => Character.PlayerName;

        public TcpClient Client { get; private set; }
        public NetworkStream Stream => Client?.GetStream();

        public AccountModel Account { get; private set; }
        public CharacterModel Character { get; private set; }

        public DateTime ConnectedAt { get; private set; }

        public Player(TcpClient client, AccountModel account, CharacterModel character)
        {
            Client = client;
            Account = account;
            Character = character;
            ConnectedAt = DateTime.UtcNow;
        }

        public void Disconnect(string reason = "Déconnecté par le serveur.")
        {
            try
            {
                Logger.Warning($"[PLAYER] Déconnexion de {Name} ({reason})");
                Stream?.Close();
                Client?.Close();
            }
            catch (Exception ex)
            {
                Logger.Error($"[PLAYER] Erreur lors de la déconnexion : {ex.Message}");
            }
        }

        public void Save()
        {
            // TODO : Implémenter la sauvegarde dans la DB
            Logger.Debug($"[PLAYER] Sauvegarde de {Name} (non implémentée)");
        }
    }
}
