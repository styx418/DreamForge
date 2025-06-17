using System.Collections.Generic;

namespace DreamCraftServer0._02.Core
{
    public static class ModuleManager
    {
        public static Dictionary<string, bool> Modules = new();

        public static void SetStatus(string name, bool status)
        {
            Modules[name] = status;
        }

        public static bool IsReady(string name) =>
            Modules.TryGetValue(name, out var status) && status;
    }
}
