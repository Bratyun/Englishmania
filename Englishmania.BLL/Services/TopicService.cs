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

        public void Create(Topic model)
        {
            bool isExist = this.IsExist(model.Name);
            if (isExist)
            {
                return;
            }

            _unitOfWork.TopicRepository.Create(model);
            _unitOfWork.Commit();
        }

        public bool IsExist(string topicName)
        {
            return null != _unitOfWork.TextRepository.Get(x => x.Name == topicName);
        }

        public Topic GetByName(string name)
        {
            var result = _unitOfWork.TopicRepository.Get(x => x.Name == name);
            if (result == null)
            {
                return new Topic();
            }

            return result;
        }
    }
}