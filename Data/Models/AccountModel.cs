using System;

namespace DreamCraftServer0._02.Data.Models
{
    public class AccountModel
    {
        public int Id { get; set; }

        public string Username { get; set; }
        public string Email { get; set; }

        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? LastLogin { get; set; }

        public string LastIP { get; set; }

        public bool IsBanned { get; set; }
        public bool IsActive { get; set; }

        public int Role { get; set; } // 0 = Joueur, 1 = Modérateur, 2 = Admin...

        public bool Online { get; set; }
    }
}
