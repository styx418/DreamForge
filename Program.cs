using System;
using System.Threading;
using DreamCraftServer0._02.Core;
using DreamCraftServer0._02.Utils;

namespace DreamCraftServer0._02
{
    internal class Program
    {
        private static bool isShuttingDown = false;
        private static Timer? shutdownTimer = null;

        static void Main(string[] args)
        {
            try
            {
                GameServer.Start();
            }
            catch (Exception ex)
            {
                Logger.Error("Le serveur a crashé de manière inattendue : " + ex.Message);
                Logger.Error(ex.StackTrace);
                Environment.Exit(1);
            }

            ListenForCommands();
        }

        private static void ListenForCommands()
        {
            while (!isShuttingDown)
            {
                Console.Write("> ");
                string? input = Console.ReadLine()?.Trim();

                if (string.IsNullOrEmpty(input))
                    continue;

                var parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                var command = parts[0].ToLower();

                switch (command)
                {
                    case "shutdown":
                        HandleShutdownCommand(parts);
                        break;

                    default:
                        Logger.Warning($"Commande inconnue : {input}");
                        break;
                }
            }
        }

        private static void HandleShutdownCommand(string[] parts)
        {
            if (parts.Length == 1 || parts[1].ToLower() == "now")
            {
                Logger.Warning("[SYSTÈME] Fermeture immédiate demandée par l’administrateur.");
                isShuttingDown = true;
                GameServer.Shutdown();
            }
            else if (int.TryParse(parts[1], out int minutes) && minutes >= 1 && minutes <= 60)
            {
                Logger.Warning($"[SYSTÈME] Fermeture dans {minutes} minute(s) demandée.");
                isShuttingDown = true;

                int totalSeconds = minutes * 60;
                shutdownTimer = new Timer(_ =>
                {
                    Logger.Warning("[SYSTÈME] Le compte à rebours est terminé. Fermeture du serveur...");
                    GameServer.Shutdown();
                }, null, totalSeconds * 1000, Timeout.Infinite);

                // Affiche les 10 dernières secondes
                new Thread(() =>
                {
                    while (totalSeconds > 0)
                    {
                        if (totalSeconds <= 10)
                        {
                            Logger.Warning($"Arrêt dans {totalSeconds} seconde(s)...");
                        }
                        Thread.Sleep(1000);
                        totalSeconds--;
                    }
                }).Start();
            }
            else
            {
                Logger.Error("Syntaxe : shutdown now  |  shutdown <minutes (1-60)>");
            }
        }
    }
}
