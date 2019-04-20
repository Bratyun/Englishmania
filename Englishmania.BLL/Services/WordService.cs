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
    public class WordService : IWordService
    {
        private readonly IUnitOfWork _unitOfWork;

        public WordService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public List<WordDto> GetByVocabulary(int userId, int vocabularyId)
        {
            List<WordVocabulary> wordVocabularies = _unitOfWork.WordVocabularyRepository.Find(x => x.VocabularyId == vocabularyId);
            List<WordDto> words = new List<WordDto>();
            if (wordVocabularies == null) return new List<WordDto>();
            foreach (var item in wordVocabularies)
            {
                Word word = _unitOfWork.WordRepository.Get(item.WordId);
                WordUser userVocabulary = _unitOfWork.WordUserRepository.Get(x => x.WordId == item.WordId && x.UserId == userId);
                if (word == null || userVocabulary == null) return new List<WordDto>();
                WordDto wordDto = new WordDto()
                {
                    Id = word.Id,
                    Count = userVocabulary.Count,
                    English = word.English,
                    Russian = word.Russian,
                    UserId = userId
                };
                words.Add(wordDto);
            }

            return words;
        }

        /// <summary>
        /// Returns count of words in vocabulary 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="vocabularyId"></param>
        /// <returns>If this vocabulary does not found, method return 0</returns>
        public int GetCountOfWords(int userId, int vocabularyId)
        {
            List<WordVocabulary> wordVocabularies =_unitOfWork.WordVocabularyRepository.Find(x => x.VocabularyId == vocabularyId);
            if (wordVocabularies == null) return 0;
            return wordVocabularies.Count;
        }

        /// <summary>
        /// Returns count of word repeat
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="wordId"></param>
        /// <returns>If this word does not found, method return 0</returns>
        public int GetProgress(int userId, int wordId)
        {
            WordUser progress = _unitOfWork.WordUserRepository.Get(x => x.UserId == userId && x.WordId == wordId);
            if (progress == null) return 0;
            return progress.Count;
        }
    }
}
