using System;

namespace Englishmania.DAL.Entities
{
    public class WordUser : BaseEntity
    {
        public int WordId { get; set; }
        public int UserId { get; set; }
        public int Count { get; set; }
        public DateTime LastUse { get; set; }
    }
}