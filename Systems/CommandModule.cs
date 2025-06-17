using System;
using System.Collections.Generic;

namespace DreamCraftServer0._02.Systems
{
    public static class CommandModule
    {
        private static readonly Dictionary<string, Action<string[]>> _commands = new();

        public static void Register(string name, Action<string[]> callback)
        {
            _commands[name.ToLower()] = callback;
        }

        public static bool Execute(string rawInput)
        {
            if (string.IsNullOrWhiteSpace(rawInput))
                return false;

            string[] parts = rawInput.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            string cmd = parts[0].ToLower();
            string[] args = parts.Length > 1 ? parts[1..] : Array.Empty<string>();

            if (_commands.TryGetValue(cmd, out var action))
            {
                action.Invoke(args);
                return true;
            }

            return false;
        }

        public static void ListAvailableCommands()
        {
            Console.WriteLine("Commandes disponibles :");
            foreach (var cmd in _commands.Keys)
            {
                Console.WriteLine($"- {cmd}");
            }
        }
    }
}
