using Englishmania.DAL.Entities;

namespace Englishmania.Web.Models
{
    public class TopicModel
    {
        public TopicModel()
        {
        }

        public TopicModel(Topic topic)
        {
            Id = topic.Id;
            Name = topic.Name;
        }

        public int Id { get; set; }
        public string Name { get; set; }
    }
}