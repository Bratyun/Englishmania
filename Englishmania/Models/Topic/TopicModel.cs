namespace Englishmania.Web.Models.Topic
{
    public class TopicModel
    {
        public TopicModel()
        {
        }

        public TopicModel(DAL.Entities.Topic topic)
        {
            Id = topic.Id;
            Name = topic.Name;
        }

        public int Id { get; set; }
        public string Name { get; set; }
    }
}