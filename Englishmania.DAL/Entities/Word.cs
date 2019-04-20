namespace Englishmania.DAL.Entities
{
    public class Word : BaseEntity
    {
        public int Id { get; set; }
        public string English { get; set; }
        public string Russian { get; set; }
    }
}
