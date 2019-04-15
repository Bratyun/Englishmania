using System;
using System.Collections.Generic;
using System.Text;

namespace Englishmania.BLL.DTO
{
    public class WordDto
    {
        public int Id { get; set; }
        public string English { get; set; }
        public string Russian { get; set; }
        public int Count { get; set; }
    }
}
