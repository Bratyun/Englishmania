using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Englishmania.BLL.Dto;
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
            List<UserVocabulary> userVocabularies = _unitOfWork.UserVocabularyRepository.Find(x => x.UserId == userId);
            if (userVocabularies == null) return new List<Vocabulary>();
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

        /// <summary>
        /// Returns progress by vocabulary
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="vocabularyId"></param>
        /// <returns>Relation learned words to all count of words</returns>
        public double GetProgress(int userId, int vocabularyId)
        {
            List<WordDto> words = _wordService.GetByVocabulary(userId, vocabularyId);
            int countOfWords = _wordService.GetCountOfWords(userId, vocabularyId);
            if (countOfWords == 0) return countOfWords;
            int learnedWords = 0;
            foreach (var word in words)
            {
                learnedWords += word.Count;
            }

            return learnedWords / (double)countOfWords;
        }
    }
}
