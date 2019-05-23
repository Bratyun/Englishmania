using System.Collections.Generic;
using Englishmania.BLL.Dto;
using Englishmania.DAL.Entities;

namespace Englishmania.BLL.Interfaces
{
    public interface IWordService
    {
        List<WordDto> GetByVocabulary(int userId, int vocabularyId);
        int GetCountOfWords(int userId, int vocabularyId);
        int GetProgress(int userId, int wordId);
        void AddToUser(int userId, int wordId);
        void Create(Word word);
        bool IsExist(string englishName);
        Word GetByEng(string englishName);
        Word Get(int wordId);
    }
}