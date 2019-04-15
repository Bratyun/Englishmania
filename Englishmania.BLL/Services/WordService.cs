using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Englishmania.BLL.DTO;
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

        public IList<WordDto> GetByVocabulary(int userId, int vocabularyId)
        {
            List<WordVocabulary> wordVocabularies = _unitOfWork.WordVocabularyRepository.Find(x => x.VocabularyId == vocabularyId).ToList();
            List<WordDto> words = new List<WordDto>();
            foreach (var item in wordVocabularies)
            {
                Word word = _unitOfWork.WordRepository.Get(item.WordId);
                WordUser userVocabulary = _unitOfWork.WordUserRepository.Get(x => x.WordId == item.WordId && x.UserId == userId);
                WordDto wordDto = new WordDto()
                {
                    Id = word.Id,
                    Count = userVocabulary.Count,
                    English = word.English,
                    Russian = word.Russian
                };
                words.Add(wordDto);
            }

            return words;
        }

        public int GetProgress(int userId, int wordId)
        {
            WordUser progress = _unitOfWork.WordUserRepository.Get(x => x.UserId == userId && x.WordId == wordId);
            if (progress != null)
            {
                return progress.Count;
            }

            return -1;
        }
    }
}
