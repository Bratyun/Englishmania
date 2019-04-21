namespace Englishmania.DAL.Entities
{
    public class WordText : BaseEntity
    {
        public int WordId { get; set; }
        public int TextId { get; set; }
        public int NumberInText { get; set; }
    }
}