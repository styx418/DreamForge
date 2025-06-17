using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DreamCraftServer0._02.Utils;
using DreamCraftServer0._02.Network.Handlers;

namespace DreamCraftServer0._02.Network
{
    public static class NetworkServer
    {
        private static TcpListener _tcpListener;
        private static UdpClient _udpClient;

        public static void Start()
        {
            try
            {
                // === TCP ===
                _tcpListener = new TcpListener(IPAddress.Any, Config.TcpPort);
                _tcpListener.Start();
                Logger.Info($"[TCP] Serveur en écoute sur le port {Config.TcpPort}");
                _ = AcceptTcpClientsAsync(); // Fire-and-forget

                // === UDP ===
                _udpClient = new UdpClient(Config.UdpPort);
                Logger.Info($"[UDP] Serveur en écoute sur le port {Config.UdpPort}");
                _ = ReceiveUdpMessagesAsync(); // Fire-and-forget
            }
            catch (Exception ex)
            {
                Logger.Error($"[ERREUR] Échec du démarrage du serveur réseau : {ex.Message}");
                throw;
            }
        }

        private static async Task AcceptTcpClientsAsync()
        {
            while (true)
            {
                try
                {
                    TcpClient client = await _tcpListener.AcceptTcpClientAsync();
                    Logger.Success($"[TCP] Nouveau client connecté : {client.Client.RemoteEndPoint}");
                    _ = HandleTcpClientAsync(client); // Fire-and-forget
                }
                catch (Exception ex)
                {
                    Logger.Warning($"[TCP] Erreur d'acceptation client : {ex.Message}");
                }
            }
        }

        private static async Task HandleTcpClientAsync(TcpClient client)
        {
            var stream = client.GetStream();
            var buffer = new byte[4096];
            var messageBuilder = new StringBuilder();

            try
            {
                while (true)
                {
                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                    if (bytesRead == 0) break; // Déconnexion

                    string received = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    messageBuilder.Append(received);

                    if (received.Contains("<EOF>"))
                    {
                        string fullMessage = messageBuilder.ToString().Replace("<EOF>", "").Trim();
                        messageBuilder.Clear(); // reset pour prochain message

                        Logger.Info($"[TCP] Message reçu : {fullMessage}");

                        if (fullMessage.StartsWith("LOGIN:"))
                        {
                            string[] parts = fullMessage.Split(':');
                            if (parts.Length >= 3)
                            {
                                string username = parts[1];
                                string password = parts[2];
                                await LoginSystem.HandleLoginAsync(client, username, password);
                            }
                            else
                            {
                                Logger.Warning("[LOGIN] Format de message invalide.");
                            }
                        }
                        else if (fullMessage.StartsWith("CREATE_CHARACTER:"))
                        {
                            string[] parts = fullMessage.Split(':');
                            if (parts.Length >= 5)
                            {
                                string name = parts[1];
                                string skin = parts[2];
                                string gender = parts[3];
                                string race = parts[4];

                                await CharacterSystem.HandleCreateCharacterAsync(client, name, skin, gender, race);
                            }
                            else
                            {
                                Logger.Warning("[CHARACTER] Requête CREATE_CHARACTER mal formée.");
                                var error = Encoding.UTF8.GetBytes("{\"success\":false,\"message\":\"Format invalide.\"}<EOF>");
                                await client.GetStream().WriteAsync(error, 0, error.Length);
                            }
                        }

                        else if (fullMessage.StartsWith("REQUEST_CHARACTERS:"))
                        {
                            string[] parts = fullMessage.Split(':');
                            if (parts.Length >= 2 && int.TryParse(parts[1], out int accId))
                            {
                                await CharacterSystem.HandleRequestCharactersAsync(client, accId);
                            }
                            else
                            {
                                Logger.Warning("[CHARACTER] Requête REQUEST_CHARACTERS mal formée.");
                                var error = Encoding.UTF8.GetBytes("{\"success\":false,\"message\":\"ID de compte invalide.\"}<EOF>");
                                await client.GetStream().WriteAsync(error, 0, error.Length);
                            }
                        }


                        else
                        {
                            Logger.Warning("[TCP] Commande inconnue : " + fullMessage);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error("[TCP] Erreur de traitement client : " + ex.Message);
            }
            finally
            {
                client.Close();
                Logger.Warning("[TCP] Client déconnecté.");
            }
        }

        private static async Task ReceiveUdpMessagesAsync()
        {
            while (true)
            {
                try
                {
                    var result = await _udpClient.ReceiveAsync();
                    Logger.Debug($"[UDP] Message reçu de {result.RemoteEndPoint}");

                    // TODO : Gérer les paquets UDP
                }
                catch (Exception ex)
                {
                    Logger.Warning($"[UDP] Erreur réception UDP : {ex.Message}");
                }
            }
        }
    }
}
