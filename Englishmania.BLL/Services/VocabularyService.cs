using System.Collections.Generic;
using Englishmania.BLL.Interfaces;
using Englishmania.DAL.Entities;
using Englishmania.DAL.Interfaces;

namespace Englishmania.BLL.Services
{
    public class VocabularyService : IVocabularyService
    {
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
        ///     Returns progress by vocabulary, relation learned words to all count of words
        /// </summary>
        /// <returns></returns>
        public double GetProgress(int userId, int vocabularyId)
        {
            var words = _wordService.GetByVocabulary(userId, vocabularyId);
            var countOfWords = _wordService.GetCountOfWords(userId, vocabularyId);
            if (countOfWords == 0) return countOfWords;
            var learnedWords = 0;
            foreach (var word in words) learnedWords += word.Count;

            return learnedWords / ((double) countOfWords * Settings.NeedToLearn);
        }

        public double GetGlobalProgress(int userId)
        {
            List<Vocabulary> vocabularies = GetByUser(userId);
            double res = 0;
            var count = 0;
            foreach (var vocabulary in vocabularies)
            {
                res += GetProgress(userId, vocabulary.Id);
                count++;
            }

            if (count == 0) return 0;
            return res / count;
        }
    }
}