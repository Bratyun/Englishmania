namespace Englishmania.BLL.Dto
{
    public class WordDto
    {
        public int Id { get; set; }
        public string English { get; set; }
        public string Russian { get; set; }
        public int Count { get; set; }
        public int UserId { get; set; }
    }
}