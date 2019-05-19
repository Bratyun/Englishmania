using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Englishmania.BLL.Dto;

namespace Englishmania.Web.Models.Vocabulary
{
    public class VocabularyWithWords
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsPrivate { get; set; }
        public int LevelId { get; set; }
        
        public List<WordDto> Words { get; set; }
    }
}
