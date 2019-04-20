namespace Englishmania.DAL.Entities
{
    public class Vocabulary : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsPrivate { get; set; }
        public int LevelId { get; set; }
    }
}
