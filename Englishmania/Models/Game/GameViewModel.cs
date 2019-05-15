using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Englishmania.Web.Models.Game
{
    public class GameViewModel
    {
        public int Id { get; set; }
        public string Russian { get; set; }
        public string English { get; set; }
        public int Progress { get; set; }
    }
}
