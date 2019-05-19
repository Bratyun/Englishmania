using System.Collections.Generic;
using System.Linq;
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

        public List<Vocabulary> GetAll()
        {
            return _unitOfWork.VocabularyRepository.GetAll().Where(x => x.IsPrivate == false).ToList();
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

        public void ConnectUserWithDictionary(int userId, int vocabularyId)
        {
            var user = _unitOfWork.UserRepository.Get(userId);
            if (user == null)
            {
                return;
            }

            var vocabulary = _unitOfWork.VocabularyRepository.Get(vocabularyId);
            if (vocabulary == null)
            {
                return;
            }

            var userVocabulary =
                _unitOfWork.UserVocabularyRepository.Get(x => x.UserId == userId && x.VocabularyId == vocabularyId);
            if (userVocabulary != null)
            {
                return;
            }

            var model = new UserVocabulary
            {
                UserId = userId,
                VocabularyId = vocabularyId
            };

            _unitOfWork.UserVocabularyRepository.Create(model);
            _unitOfWork.Commit();

            var words = _wordService.GetByVocabulary(userId, vocabularyId);
            foreach (var word in words)
            {
                _wordService.AddToUser(userId, word.Id);
            }
        }

        public void Create(Vocabulary model)
        {
            bool isExist = this.IsExist(model.Name);
            if (isExist)
            {
                return;
            }

            _unitOfWork.VocabularyRepository.Create(model);
            _unitOfWork.Commit();
        }

        public bool IsExist(string vocabularyName)
        {
            return null != _unitOfWork.VocabularyRepository.Get(x => x.Name == vocabularyName);
        }

        public void AddWord(int wordId, int vocabularyId)
        {
            var word = _unitOfWork.WordRepository.Get(wordId);
            var vocabulary = _unitOfWork.VocabularyRepository.Get(vocabularyId);
            if (word == null || vocabulary == null)
            {
                return;
            }

            var old = _unitOfWork.WordVocabularyRepository.Get(
                x => x.WordId == wordId && x.VocabularyId == vocabularyId);
            if (old != null)
            {
                return;
            }

            var model = new WordVocabulary
            {
                WordId = wordId,
                VocabularyId = vocabularyId
            };
            _unitOfWork.WordVocabularyRepository.Create(model);
            _unitOfWork.Commit();
        }

        public Vocabulary GetByName(string name)
        {
            bool res = this.IsExist(name);
            if (res)
            {
                return _unitOfWork.VocabularyRepository.Get(x => x.Name == name);
            }

            return new Vocabulary();
        }
    }
}