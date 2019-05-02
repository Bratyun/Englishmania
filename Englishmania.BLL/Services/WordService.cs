using System;
using System.Collections.Generic;
using System.Linq;
using Englishmania.BLL.Dto;
using Englishmania.BLL.Interfaces;
using Englishmania.DAL.Entities;
using Englishmania.DAL.Interfaces;

namespace Englishmania.BLL.Services
{
    public class WordService : IWordService
    {
        private readonly IUnitOfWork _unitOfWork;

        public WordService(IUnitOfWork unitOfWork, ILevelService levelService)
        {
            _unitOfWork = unitOfWork;
        }

        public List<WordDto> GetByVocabulary(int userId, int vocabularyId)
        {
            var wordVocabularies = _unitOfWork.WordVocabularyRepository.Find(x => x.VocabularyId == vocabularyId);
            var words = new List<WordDto>();
            if (wordVocabularies == null) return new List<WordDto>();
            foreach (var item in wordVocabularies)
            {
                var wordDto = new WordDto(item.WordId, userId, _unitOfWork);
                words.Add(wordDto);
            }

            return words;
        }

        /// <summary>
        ///     Returns count of words in vocabulary, if this vocabulary does not found, method return 0
        /// </summary>
        public int GetCountOfWords(int userId, int vocabularyId)
        {
            var wordVocabularies = _unitOfWork.WordVocabularyRepository.Find(x => x.VocabularyId == vocabularyId);
            if (wordVocabularies == null) return 0;
            return wordVocabularies.Count;
        }

        /// <summary>
        ///     Returns count of word repeat, if this word does not found, method return 0
        /// </summary>
        public int GetProgress(int userId, int wordId)
        {
            var progress = _unitOfWork.WordUserRepository.Get(x => x.UserId == userId && x.WordId == wordId);
            if (progress == null) return 0;
            return progress.Count;
        }
    }
}