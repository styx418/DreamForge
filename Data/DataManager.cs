using DreamCraftServer0._02.Entities;
using DreamCraftServer0._02.Utils;

namespace DreamCraftServer0._02.Data
{
    public static class DataManager
    {
        public static void SaveAll()
        {
            Logger.Info("[DATA MANAGER] Sauvegarde de tous les joueurs...");
            PlayerManager.SaveAll();
            Logger.Success("[DATA MANAGER] Sauvegarde terminée.");
        }
    }
}
