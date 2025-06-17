using System;
using System.Data;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DreamCraftServer0._02.Data;
using DreamCraftServer0._02.Utils;
using MySql.Data.MySqlClient;

namespace DreamCraftServer0._02.Network.Handlers
{
    public static class CharacterSystem
    {
        public static async Task HandleCreateCharacterAsync(TcpClient client, string name, string skin, string gender, string race)
        {
            try
            {
                using var connection = Database.GetConnection();
                await connection.OpenAsync();

                // Vérifier si le nom est déjà pris
                var checkCmd = new MySqlCommand("SELECT COUNT(*) FROM characters WHERE Player_Name = @name", connection);
                checkCmd.Parameters.AddWithValue("@name", name);
                long count = (long)await checkCmd.ExecuteScalarAsync();
                if (count > 0)
                {
                    await SendJson(client, new { success = false, message = "Nom de personnage déjà utilisé." });
                    return;
                }

                //  Remplacer acc_id par la vraie session une fois dispo
                int accId = 1;

                var insertCmd = new MySqlCommand(@"
                    INSERT INTO characters 
                    (acc_id, Player_Name, Player_Skin, Gender, Race, Level, Xp, XpToLevel)
                    VALUES 
                    (@acc_id, @name, @skin, @gender, @race, 1, 0, 100)", connection);

                insertCmd.Parameters.AddWithValue("@acc_id", accId);
                insertCmd.Parameters.AddWithValue("@name", name);
                insertCmd.Parameters.AddWithValue("@skin", skin);
                insertCmd.Parameters.AddWithValue("@gender", gender);
                insertCmd.Parameters.AddWithValue("@race", race);

                await insertCmd.ExecuteNonQueryAsync();

                Logger.Success($"[CHARACTER] Création réussie : {name}");
                await SendJson(client, new { success = true, message = "Personnage créé avec succès." });
            }
            catch (Exception ex)
            {
                Logger.Error("[CHARACTER] Erreur serveur : " + ex.Message);
                await SendJson(client, new { success = false, message = "Erreur lors de la création du personnage." });
            }
        }

        private static async Task SendJson(TcpClient client, object data)
        {
            var json = JsonSerializer.Serialize(data) + "<EOF>";
            var buffer = Encoding.UTF8.GetBytes(json);
            await client.GetStream()
                        .WriteAsync(buffer, 0, buffer.Length);
        }


        public static async Task HandleRequestCharactersAsync(TcpClient client, int accId)
        {
            try
            {
                using var connection = Database.GetConnection();
                await connection.OpenAsync();

                var cmd = new MySqlCommand(@"
            SELECT Player_Name, Player_Skin, Level, Race, Gender
            FROM characters
            WHERE acc_id = @acc_id AND is_deleted = 0", connection);
                cmd.Parameters.AddWithValue("@acc_id", accId);

                var characters = new List<object>();
                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    characters.Add(new
                    {
                        name = reader.GetString("Player_Name"),
                        skin = reader.GetString("Player_Skin"),
                        level = reader.GetInt32("Level"),
                        race = reader.GetString("Race"),
                        gender = reader.GetString("Gender")
                    });
                }

                var response = new
                {
                    success = true,
                    characters = characters
                };

                await SendJson(client, response);
            }
            catch (Exception ex)
            {
                Logger.Error("[CHARACTER] Erreur lors de la récupération : " + ex.Message);
                await SendJson(client, new { success = false, message = "Erreur serveur." });
            }
        }





    }
}
