using System;
using System.Collections.Generic;
using System.Text;
using Englishmania.DAL.Entities;

namespace Englishmania.BLL.Dto
{
    public class WordWithLevelDto
    {
        public int Id { get; set; }
        public string English { get; set; }
        public string Russian { get; set; }
        public int Count { get; set; }
        public int UserId { get; set; }
        public int LevelDto { get; set; }
    }
}
