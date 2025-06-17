using System;

namespace DreamCraftServer0._02.Data.Models
{
    public class CharacterModel
    {
        public int CharacterId { get; set; }
        public int AccountId { get; set; }

        public string PlayerName { get; set; }
        public string PlayerSkin { get; set; }

        public string Gender { get; set; }  // Male / Female
        public string Race { get; set; }    // Elf, Human, Orc...
        public string Class { get; set; }   // Warrior, Mage...

        public int Level { get; set; }
        public int Xp { get; set; }
        public int XpToLevel { get; set; }

        public float PosX { get; set; }
        public float PosY { get; set; }
        public float PosZ { get; set; }

        public int Str { get; set; }
        public int Endu { get; set; }
        public int AC { get; set; }
        public int Agil { get; set; }
        public int Intel { get; set; }
        public int Sages { get; set; }

        public int ZoneID { get; set; }
        public int MapID { get; set; }

        public bool IsDeleted { get; set; }
    }
}
