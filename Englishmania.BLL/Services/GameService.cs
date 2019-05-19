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
    public class GameService : IGameService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly Random _random;
        private readonly ILevelService _levelService;

        public GameService(IUnitOfWork unitOfWork, ILevelService levelService)
        {
            _unitOfWork = unitOfWork;
            _random = new Random((int)DateTime.UtcNow.Ticks);
            _levelService = levelService;
        }

        public GameTranslationDto GetTranslationGameFromEngToRus(int userId)
        {
            List<Word> words = WordsForGameTranslation(userId);
            if (words.Count < 4) return new GameTranslationDto();

            int a = 0, b = 0, c = 0, d = 0;
            while (!((a != b) && (a != c) && (a != d) && (b != c) && (b != d) && (c != d)))
            {
                a = _random.Next(0, words.Count);
                b = _random.Next(0, words.Count);
                c = _random.Next(0, words.Count);
                d = _random.Next(0, words.Count);
            }

            var result = new GameTranslationDto
            {
                Translation = new List<string>()
            };
            var wordDto = new WordDto(a, userId, _unitOfWork);
            result.Word = wordDto;
            result.Translation.Add(words[b].Russian);
            result.Translation.Add(words[c].Russian);
            result.Translation.Add(words[d].Russian);
            return result;
        }

        public GameTranslationDto GetTranslationGameFromRusToEng(int userId)
        {
            List<Word> words = WordsForGameTranslation(userId);
            if (words.Count < 4) return new GameTranslationDto();

            int a = 0, b = 0, c = 0, d = 0;
            while (a != b && a != c && a != d && b != c && b != d && c != d)
            {
                a = _random.Next(0, words.Count);
                b = _random.Next(0, words.Count);
                c = _random.Next(0, words.Count);
                d = _random.Next(0, words.Count);
            }

            var result = new GameTranslationDto
            {
                Translation = new List<string>()
            };
            var wordDto = new WordDto(a, userId, _unitOfWork);
            result.Word = wordDto;
            result.Translation.Add(words[b].English);
            result.Translation.Add(words[c].English);
            result.Translation.Add(words[d].English);
            return result;
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

        private List<Word> WordsForGameTranslation(int userId)
        {
            int newWords = _random.Next(0, 101);
            var user = _unitOfWork.UserRepository.Get(userId);
            if (user == null) return new List<Word>();
            List<WordUser> wordUsers = newWords <= 25 ? _unitOfWork.WordUserRepository.Find(x => x.UserId == userId && x.Count == 0) : GetWordsByUserForTranslationGame(userId);

            List<Word> words = new List<Word>();
            foreach (var item in wordUsers)
            {
                var word = _unitOfWork.WordRepository.Get(item.WordId);
                if (word == null) continue;
                words.Add(word);
            }

            if (words.Count < 4) return words;

            var resultWords = ChooseByLevel(user, words);
            if (resultWords.Count < 4) return words;
            return resultWords;
        }

        private List<Word> ChooseByLevel(User user, List<Word> words)
        {
            int hardLevel = _random.Next(0, 101);
            List<Word> result = new List<Word>();
            int currentLevel = user.LevelId;

            foreach (var word in words)
            {
                var level = _levelService.GetByWord(user.Id, word.Id);
                if (level == null) continue;
                if (hardLevel >= 0 && hardLevel < 15)
                    if (level.Id < currentLevel)
                        result.Add(word);
                if (hardLevel >= 15 && hardLevel < 85)
                    if (level.Id == currentLevel)
                        result.Add(word);
                if (hardLevel >= 85 && hardLevel <= 100)
                    if (level.Id > currentLevel)
                        result.Add(word);
            }

            return result;
        }

        private List<WordUser> GetWordsByUserForTranslationGame(int userId)
        {
            List<WordUser> wordUsers = _unitOfWork.WordUserRepository.Find(x => x.UserId == userId && x.Count < Settings.NeedToLearn);
            if (wordUsers.Count < 4) return new List<WordUser>();

            var wordUsersLevelThree = wordUsers.Where(x => x.Count >= 3 && x.LastUse < DateTime.UtcNow && x.LastUse.AddMonths(3) < DateTime.UtcNow).ToList();
            if (wordUsersLevelThree.Count < 4) return wordUsers;

            var wordUsersLevelTwo = wordUsers.Where(x => x.Count >= 2 && x.LastUse < DateTime.UtcNow && x.LastUse.AddDays(7) < DateTime.UtcNow).ToList();
            if (wordUsersLevelTwo.Count < 4) return wordUsersLevelThree;

            var wordUsersLevelOne = wordUsers.Where(x => x.Count >= 1 && x.LastUse < DateTime.UtcNow && x.LastUse.AddDays(1) < DateTime.UtcNow).ToList();
            if (wordUsersLevelOne.Count < 4) return wordUsersLevelTwo;
            return wordUsersLevelOne;
        }
    }
}
