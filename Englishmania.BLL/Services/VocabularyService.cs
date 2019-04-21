using System.Collections.Generic;
using Englishmania.BLL.Interfaces;
using Englishmania.DAL.Entities;
using Englishmania.DAL.Interfaces;

namespace Englishmania.BLL.Services
{
    public class VocabularyService : IVocabularyService
    {
        private const int NeedToLearn = 4;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWordService _wordService;

        public VocabularyService(IUnitOfWork unitOfWork, IWordService wordService)
        {
            _unitOfWork = unitOfWork;
            _wordService = wordService;
        }

        public List<Vocabulary> GetByUser(int userId)
        {
            var userVocabularies = _unitOfWork.UserVocabularyRepository.Find(x => x.UserId == userId);
            if (userVocabularies == null) return new List<Vocabulary>();
            var vocabularies = new List<Vocabulary>();
            foreach (var item in userVocabularies)
            {
                var vocabulary = _unitOfWork.VocabularyRepository.Get(item.VocabularyId);
                if (vocabulary != null) vocabularies.Add(vocabulary);
            }

            return vocabularies;
        }

        /// <summary>
        ///     Returns progress by vocabulary
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="vocabularyId"></param>
        /// <returns>Relation learned words to all count of words</returns>
        public double GetProgress(int userId, int vocabularyId)
        {
            var words = _wordService.GetByVocabulary(userId, vocabularyId);
            var countOfWords = _wordService.GetCountOfWords(userId, vocabularyId);
            if (countOfWords == 0) return countOfWords;
            var learnedWords = 0;
            foreach (var word in words) learnedWords += word.Count;

            return learnedWords / ((double) countOfWords * NeedToLearn);
        }
    }
}