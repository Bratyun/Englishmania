using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Englishmania.Web.Models.Game
{
    public class GameWithTranslationViewModel
    {
        public GameViewModel Game { get; set; }
        public List<string> Translations { get; set; }
    }
}
