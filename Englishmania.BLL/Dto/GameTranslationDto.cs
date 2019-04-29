using System;
using System.Collections.Generic;
using System.Text;

namespace Englishmania.BLL.Dto
{
    public class GameTranslationDto
    {
        public WordDto Word { get; set; }
        public List<string> Translation { get; set; }
    }
}
