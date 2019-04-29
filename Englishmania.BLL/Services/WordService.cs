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
        private readonly Random _random;
        private readonly ILevelService _levelService;

        public WordService(IUnitOfWork unitOfWork, ILevelService levelService)
        {
            _unitOfWork = unitOfWork;
            _random = new Random((int)DateTime.UtcNow.Ticks);
            _levelService = levelService;
        }

        public List<WordDto> GetByVocabulary(int userId, int vocabularyId)
        {
            var wordVocabularies = _unitOfWork.WordVocabularyRepository.Find(x => x.VocabularyId == vocabularyId);
            var words = new List<WordDto>();
            if (wordVocabularies == null) return new List<WordDto>();
            foreach (var item in wordVocabularies)
            {
                var word = _unitOfWork.WordRepository.Get(item.WordId);
                var userVocabulary =
                    _unitOfWork.WordUserRepository.Get(x => x.WordId == item.WordId && x.UserId == userId);
                if (word == null || userVocabulary == null) return new List<WordDto>();
                var wordDto = new WordDto
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

        public GameTranslationDto GetTranslationGameFromEngToRus(int userId)
        {
            int newWords = _random.Next(0, 101);
            var user = _unitOfWork.UserRepository.Get(userId);
            if (user == null) return new GameTranslationDto();
            List<WordUser> wordUsers = GetWordsByUserForTranslationGame(userId);
            List<Word> words = new List<Word>();
            foreach (var item in wordUsers)
            {
                var word = _unitOfWork.WordRepository.Get(item.WordId);
                if (word == null) continue;
                words.Add(word);
            }

            return null;
        }
        
        //TODO надо делать тут
        private List<Word> ChooseByLevel(User user, List<Word> words)
        {
            int hardLevel = _random.Next(0, 101);
            List<Word> result = new List<Word>();
            
            foreach (var word in words)
            {
                var level = _levelService.GetByWord(user.Id, word.Id);
                if (level == null) continue;
                
            }

            return result;
        }

        private List<WordUser> GetWordsByUserForTranslationGame(int userId)
        {
            List<WordUser> wordUsers = _unitOfWork.WordUserRepository.Find(x => x.UserId == userId && x.Count < Settings.NeedToLearn);
            if (wordUsers.Count < 4) return null;

            var wordUsersLevelThree = wordUsers.Where(x => x.Count >= 3 && x.LastUse < DateTime.UtcNow && x.LastUse.AddMonths(3) < DateTime.UtcNow).ToList();
            if (wordUsersLevelThree.Count < 4) return wordUsers;

            var wordUsersLevelTwo = wordUsers.Where(x => x.Count >= 2 && x.LastUse < DateTime.UtcNow && x.LastUse.AddDays(7) < DateTime.UtcNow).ToList();
            if (wordUsersLevelTwo.Count < 4) return wordUsersLevelThree;

            var wordUsersLevelOne = wordUsers.Where(x => x.Count >= 1 && x.LastUse < DateTime.UtcNow && x.LastUse.AddDays(1) < DateTime.UtcNow).ToList();
            if (wordUsersLevelOne.Count < 4) return wordUsersLevelTwo;
            return wordUsersLevelOne;
        }

        public GameTranslationDto GetTranslationGameFromRusToEng(int userId)
        {
            throw new System.NotImplementedException();
        }

        public void SetWordProgress(int userId, GameTranslationResult model)
        {
            var word = _unitOfWork.WordUserRepository.Get(x => x.UserId == userId && x.WordId == model.WordId);
            if (word == null) return;
            word.LastUse = DateTime.UtcNow;
            word.Count = model.IsSuccess ? word.Count + 1 : word.Count - 1;
            if (word.Count <= 0) word.Count = 0;
            _unitOfWork.WordUserRepository.Update(word);
            _unitOfWork.Commit();
        }
    }
}