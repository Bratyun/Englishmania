using System;
using System.Collections.Generic;
using System.Linq;
using Englishmania.BLL.Interfaces;
using Englishmania.DAL.Entities;
using Englishmania.DAL.Interfaces;

namespace Englishmania.BLL.Services
{
    public class LevelService : ILevelService
    {
        private readonly IUnitOfWork _unitOfWork;

        public LevelService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Level GetByWord(int userId, int wordId)
        {
            var userVocabulary = _unitOfWork.UserVocabularyRepository.GetAll();
            var vocabularies = _unitOfWork.VocabularyRepository.GetAll();
            var wordVocabulary = _unitOfWork.WordVocabularyRepository.GetAll();
            var result = from v in vocabularies
                join uv in userVocabulary on v.Id equals uv.VocabularyId
                join wv in wordVocabulary on v.Id equals wv.VocabularyId
                where uv.UserId == userId && wv.WordId == wordId
                select v;
            if (result.ToList().Count == 0) return null;
            
            return _unitOfWork.LevelRepository.Get(result.FirstOrDefault().LevelId);

        }
    }
}
