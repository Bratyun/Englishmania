namespace Englishmania.DAL.Entities
{
    public class Text : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Body { get; set; }
        public int LevelId { get; set; }
    }
}
