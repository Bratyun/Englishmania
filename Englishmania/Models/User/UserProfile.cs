using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Englishmania.Web.Models.User
{
    public class UserProfile
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
        public int LevelId { get; set; }
    }
}
