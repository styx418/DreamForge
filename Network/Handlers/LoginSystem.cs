

 using System.Collections.Generic;
 using System.Data;
 using System.Net.Sockets;
 using System.Security.Cryptography;
 using System.Text;
 using System.Text.Json;
 using System.Threading.Tasks;
 using DreamCraftServer0._02.Data;
 using DreamCraftServer0._02.Data.Models;
 using DreamCraftServer0._02.Utils;
 using MySql.Data.MySqlClient;
 
 namespace DreamCraftServer0._02.Network
{
    public static class LoginSystem
    {
        public static async Task HandleLoginAsync(TcpClient client, string username, string password)
        {
            Logger.Info($"[LOGIN] Tentative de connexion pour : {username}");

            try
            {
                using var connection = Database.GetConnection();
                await connection.OpenAsync();

                
                var cmd = new MySqlCommand(@"SELECT id, email, password, account_level, is_active, is_banned FROM accounts WHERE username = @username", connection);
                cmd.Parameters.AddWithValue("@username", username);

                using var reader = await cmd.ExecuteReaderAsync();

                if (!reader.HasRows)
                {
                    await SendResponse(client, new { success = false, message = "Utilisateur inconnu." });
                    Logger.Warning($"[LOGIN] Utilisateur inexistant : {username}");
                    return;
                }

                await reader.ReadAsync();

               
                string storedPassword = reader.GetString("password");

                bool isActive = reader.GetBoolean("is_active");
                bool isBanned = reader.GetBoolean("is_banned");

                int id = reader.GetInt32("id");
                string email = reader.GetString("email");
               
                int accountLevel = reader.GetInt32("account_level");

                string inputHash = ComputeHash(password, salt);
                         
                                    if (password != storedPassword)
                {
                    await SendResponse(client, new { success = false, message = "Mot de passe incorrect." });
                    Logger.Warning($"[LOGIN] Mot de passe incorrect pour : {username}");
                    return;
                }

                if (!isActive)
                {
                    await SendResponse(client, new { success = false, message = "Compte inactif." });
                    Logger.Warning($"[LOGIN] Compte inactif pour : {username}");
                    return;
                }

                if (isBanned)
                {
                    await SendResponse(client, new { success = false, message = "Compte banni." });
                    Logger.Warning($"[LOGIN] Compte banni : {username}");
                    return;
                }

                string ip = ((System.Net.IPEndPoint)client.Client.RemoteEndPoint).Address.ToString();
                reader.Close();

                var updateCmd = new MySqlCommand(@"
-                    UPDATE accounts 
-                    SET last_login = CURRENT_TIMESTAMP, 
-                        last_ip = @ip, 
-                        online = 1 
+                    UPDATE accounts
+                    SET last_loggin = CURRENT_TIMESTAMP,
+                        online = 1
                     WHERE id = @id", connection);
              
                updateCmd.Parameters.AddWithValue("@id", id);
                await updateCmd.ExecuteNonQueryAsync();

                Logger.Success($"[LOGIN] {username} connecté depuis {ip}");

                var characterList = new List<object>();
                using var charCmd = new MySqlCommand("SELECT Player_Name, Player_Skin, Level, Gender, Race FROM characters WHERE acc_id = @acc_id AND is_deleted = 0", connection);
                charCmd.Parameters.AddWithValue("@acc_id", id);

                using var charReader = await charCmd.ExecuteReaderAsync();
                while (await charReader.ReadAsync())
                {
                    characterList.Add(new
                    {
                        name = charReader.GetString("Player_Name"),
                        skin = charReader.GetString("Player_Skin"),
                        level = charReader.GetInt32("Level"),
                        gender = charReader.GetString("Gender"),
                        race = charReader.GetString("Race")
                    });
                }

                Logger.Info($"[LOGIN] {characterList.Count} personnage(s) trouvé(s) pour {username}");

                var account = new AccountModel
                {
                    Id = id,
                    Username = username,
                    Email = email,
                    -Role = role,
                    +Role = accountLevel,
                    IsActive = isActive,
                    IsBanned = isBanned,
                    -LastIP = ip,
                    -LastLogin = DateTime.Now,
                    Online = true
                };

                await SendResponse(client, new
                {
                    success = true,
                    message = "Connexion réussie.",
                    account,
                    characters = characterList
                });
            }
            catch (Exception ex)
            {
                Logger.Error($"[LOGIN] Exception : {ex.Message}");
                await SendResponse(client, new { success = false, message = "Erreur serveur." });
            }
        }

        private static async Task SendResponse(TcpClient client, object data)
        {
            var json = JsonSerializer.Serialize(data) + "<EOF>";
            var buffer = Encoding.UTF8.GetBytes(json);
            await client.GetStream().WriteAsync(buffer, 0, buffer.Length);
        }


    }
}