using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamCraftServer0._02.Data.Models
{
    public class LoginRequest
    {
        public string type { get; set; }
        public string username { get; set; }
        public string password { get; set; }
    }
}