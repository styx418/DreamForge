using System;
using System.Threading;
using DreamCraftServer0._02.Data;
using DreamCraftServer0._02.Entities;
//using DreamCraftServer0._02.Managers;
using DreamCraftServer0._02.Network;
using DreamCraftServer0._02.Utils;

namespace DreamCraftServer0._02.Core
{
    public static class GameServer
    {
        public static void Start()
        {
            Console.Clear();
            Banner.Show();

            Logger.Info("Démarrage du cœur du serveur...");
            AnimateStartup("Démarrage du serveur");

            InitializeModules();

            Logger.Section("Vérification des modules");
            foreach (var module in ModuleManager.Modules)
            {
                if (module.Value)
                    Logger.Success($"[OK] {module.Key}");
                else
                    Logger.Error($"[ERREUR] {module.Key}");
            }

            if (ModuleManager.Modules.ContainsValue(false))
            {
                Logger.Error("❌ Un ou plusieurs modules ont échoué. Arrêt du serveur.");
                Shutdown();
                return;
            }

            Logger.Line();
            Logger.Success("✅ DreamCraftServer initialisé avec succès. Le serveur est opérationnel.");
            Logger.Line();
            Console.WriteLine();
        }

        private static void AnimateStartup(string message)
        {
            Console.Write($"{message}");
            for (int i = 0; i < 5; i++)
            {
                Thread.Sleep(500);
                Console.Write(".");
            }
            Console.WriteLine();
        }

        private static void InitializeModules()
        {
            // === Base de Données ===
            Logger.Section("Initialisation : Base de Données");
            if (Database.Initialize() && Database.TestConnection())
            {
                ModuleManager.SetStatus("Base de Données", true);
                Logger.Success("[OK] Connexion à la base de données établie.");
            }
            else
            {
                ModuleManager.SetStatus("Base de Données", false);
                Logger.Error("[ERREUR] Impossible de se connecter à la base de données.");
            }

            Thread.Sleep(250);

            // === Réseau ===
            Logger.Section("Initialisation : Réseau");
            try
            {
                NetworkServer.Start();
                ModuleManager.SetStatus("Serveur Réseau", true);
                Logger.Success("[OK] Serveur TCP/UDP lancé avec succès.");
            }
            catch (Exception ex)
            {
                ModuleManager.SetStatus("Serveur Réseau", false);
                Logger.Error("[ERREUR] Lancement du serveur réseau échoué : " + ex.Message);
            }

            Thread.Sleep(250);
        }

        public static void Shutdown()
        {
            Logger.Warning("Arrêt du serveur en cours...");

            // Sauvegarde
            try
            {
                Logger.Info("Sauvegarde des données en cours...");
                DataManager.SaveAll();
                Logger.Success("[OK] Données sauvegardées.");
            }
            catch (Exception ex)
            {
                Logger.Error("[ERREUR] Sauvegarde échouée : " + ex.Message);
            }

            // Déconnexion
            try
            {
                Logger.Info("Déconnexion des joueurs...");
                PlayerManager.DisconnectAll();
                Logger.Success("[OK] Tous les joueurs ont été déconnectés.");
            }
            catch (Exception ex)
            {
                Logger.Error("[ERREUR] Échec de la déconnexion des joueurs : " + ex.Message);
            }

            Logger.Success("✅ Le serveur s'est arrêté proprement.");
            Thread.Sleep(1000);
            Environment.Exit(0);
        }
    }
}
