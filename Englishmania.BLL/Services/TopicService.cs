using System.Collections.Generic;
using Englishmania.BLL.Interfaces;
using Englishmania.DAL.Entities;
using Englishmania.DAL.Interfaces;

namespace Englishmania.BLL.Services
{
    public class TopicService : ITopicService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TopicService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public List<Topic> GetByVocabulary(int vocabularyId)
        {
            var mixed =
                _unitOfWork.TopicVocabularyRepository.Find(x => x.VocabularyId == vocabularyId);
            if (mixed == null) return new List<Topic>();
            var results = new List<Topic>();
            foreach (var item in mixed)
            {
                var topic = _unitOfWork.TopicRepository.Get(item.TopicId);
                if (topic != null) results.Add(topic);
            }

            return results;
        }
    }
}