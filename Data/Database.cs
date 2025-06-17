using System;
using System.Data;
using DreamCraftServer0._02.Utils;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System.IO;

namespace DreamCraftServer0._02.Data
{
    public static class Database
    {
        private static string _connectionString;

        public static bool Initialize()
        {
            try
            {
                var config = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .Build();

                var dbSection = config.GetSection("Database");
                var host = dbSection["Host"];
                var user = dbSection["User"];
                var password = dbSection["Password"];
                var database = dbSection["Database"];
                var port = dbSection["Port"];

                _connectionString = $"Server={host};Port={port};Database={database};Uid={user};Pwd={password};SslMode=none;";

                Logger.Success("[DATABASE] Configuration chargée avec succès.");
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error("[DATABASE] Erreur lors du chargement de la configuration : " + ex.Message);
                return false;
            }
        }

        public static MySqlConnection GetConnection()
        {
            if (string.IsNullOrWhiteSpace(_connectionString))
            {
                throw new InvalidOperationException("La base de données n'a pas été initialisée.");
            }

            return new MySqlConnection(_connectionString);
        }

        public static bool TestConnection()
        {
            try
            {
                using var connection = GetConnection();
                connection.Open();
                Logger.Success("[DATABASE] Connexion à MySQL réussie.");
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error("[DATABASE] Échec de la connexion à MySQL : " + ex.Message);
                return false;
            }
        }
    }
}
