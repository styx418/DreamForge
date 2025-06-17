using System;
using System.IO;

namespace DreamCraftServer0._02.Utils
{
    public static class Logger
    {
        private static readonly string logDirectory = "logs";
        private static readonly object logLock = new();

        static Logger()
        {
            // Crée le dossier si nécessaire
            if (!Directory.Exists(logDirectory))
                Directory.CreateDirectory(logDirectory);
        }

        private static string GetLogFilePath()
        {
            string date = DateTime.Now.ToString("yyyy-MM-dd");
            return Path.Combine(logDirectory, $"{date}.txt");
        }

        private static void WriteLog(string level, string message, ConsoleColor color)
        {
            string timestamp = DateTime.Now.ToString("HH:mm:ss");
            string fullMessage = $"[{timestamp}] [{level}] {message}";

            lock (logLock)
            {
                // Console
                Console.ForegroundColor = color;
                Console.Write($"[{timestamp}] ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write($"[{level}] ");
                Console.ResetColor();
                Console.WriteLine(message);

                // Fichier du jour
                File.AppendAllText(GetLogFilePath(), fullMessage + Environment.NewLine);
            }
        }

        public static void Info(string message) => WriteLog("INFO", message, ConsoleColor.Cyan);
        public static void Success(string message) => WriteLog("SUCCESS", message, ConsoleColor.Green);
        public static void Warning(string message) => WriteLog("WARNING", message, ConsoleColor.Yellow);
        public static void Error(string message) => WriteLog("ERROR", message, ConsoleColor.Red);
        public static void Debug(string message) => WriteLog("DEBUG", message, ConsoleColor.Magenta);

        public static void Section(string message)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(message);
            Console.ResetColor();
            File.AppendAllText(GetLogFilePath(), message + Environment.NewLine);
        }

        public static void Headline(string message)
        {
            string headline = $"=== {message.ToUpper()} ===";
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\n" + headline);
            Console.ResetColor();
            File.AppendAllText(GetLogFilePath(), headline + Environment.NewLine);
        }
        public static void Line()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(new string('─', 80));
            Console.ResetColor();
        }



        public static void Break()
        {
            Console.WriteLine();
            File.AppendAllText(GetLogFilePath(), Environment.NewLine);
        }
    }
}
