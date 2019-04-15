using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Englishmania.BLL.Interfaces;
using Englishmania.DAL.Entities;
using Englishmania.DAL.Interfaces;

namespace Englishmania.BLL.Services
{
    public class VocabularyService : IVocabularyService
    {
        private readonly IUnitOfWork _unitOfWork;

        public VocabularyService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IList<Vocabulary> GetByUser(int userId)
        {
            List<UserVocabulary> userVocabularies = _unitOfWork.UserVocabularyRepository.Find(x => x.UserId == userId).ToList();
            List<Vocabulary> vocabularies = new List<Vocabulary>();
            foreach (var item in userVocabularies)
            {
                Vocabulary vocabulary = _unitOfWork.VocabularyRepository.Get(item.VocabularyId);
                if (vocabulary != null)
                {
                    vocabularies.Add(vocabulary);
                }
            }

            return vocabularies;
        }

        public double GetProgress(int userId, int vocabularyId)
        {
            throw new NotImplementedException();
        }
    }
}
