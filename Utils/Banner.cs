using System;
using System.IO;

namespace DreamCraftServer0._02.Utils
{
    public static class Banner
    {
        public static string Version => File.Exists("version.txt")
            ? File.ReadAllText("version.txt").Trim()
            : "Build ???";

        public static void Show()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(@"
  ____   ____    _____     /\    __  __   ____   ____      /\    _____   _____
 |  _ \ |  _ \  | ____|   /  \  |  \/  | / ___| |  _ \    /  \  |  ___| |_   _|
 | | | || |_) | |  _|    / /\ \ | |\/| || |     | |_) |  / /\ \ | |_      | |
 | |_| ||  _ <  | |___  / ____ \| |  | || |___  |  _ <  / ____ \|  _|     | |
 |____/ |_| \_\ |_____| /_/  \_\|_|  |_| \____| |_| \_\ /_/  \_\|_|       |_|
");

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"                      DreamCraft Server {Version}\n");

            Console.ResetColor();
        }
    }
}
