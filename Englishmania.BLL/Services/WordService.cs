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
        
        public int GetCountOfWords(int userId, int vocabularyId)
        {
            var wordVocabularies = _unitOfWork.WordVocabularyRepository.Find(x => x.VocabularyId == vocabularyId);
            if (wordVocabularies == null) return 0;
            return wordVocabularies.Count;
        }
        
        public int GetProgress(int userId, int wordId)
        {
            var progress = _unitOfWork.WordUserRepository.Get(x => x.UserId == userId && x.WordId == wordId);
            if (progress == null) return 0;
            return progress.Count;
        }

        public void AddToUser(int userId, int wordId)
        {
            var user = _unitOfWork.UserRepository.Get(userId);
            var word = _unitOfWork.WordRepository.Get(wordId);
            if (word == null || user == null)
            {
                return;
            }

            var old = _unitOfWork.WordUserRepository.Get(x => x.UserId == userId && x.WordId == wordId);
            if (old != null)
            {
                return;
            }

            var model = new WordUser
            {
                UserId = userId,
                WordId = wordId,
                Count = 0,
                LastUse = DateTime.UtcNow
            };
            _unitOfWork.WordUserRepository.Create(model);
            _unitOfWork.Commit();
        }

        public void Create(Word word)
        {
            bool isExist = this.IsExist(word.English);
            if (isExist)
            {
                return;
            }

            _unitOfWork.WordRepository.Create(word);
            _unitOfWork.Commit();
        }

        public bool IsExist(string englishName)
        {
            return null != _unitOfWork.WordRepository.Get(x => x.English == englishName);
        }

        public Word GetByEng(string englishName)
        {
            bool res = this.IsExist(englishName);
            if (res)
            {
                return _unitOfWork.WordRepository.Get(x => x.English == englishName);
            }

            return new Word();
        }
    }
}