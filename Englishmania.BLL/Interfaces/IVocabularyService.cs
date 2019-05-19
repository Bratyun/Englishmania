using System.Collections.Generic;
using Englishmania.DAL.Entities;

namespace Englishmania.BLL.Interfaces
{
    public interface IVocabularyService
    {
        List<Vocabulary> GetAll();
        List<Vocabulary> GetByUser(int userId);
        double GetProgress(int userId, int vocabularyId);
        double GetGlobalProgress(int userId);
        void ConnectUserWithDictionary(int userId, int vocabularyId);
        void Create(Vocabulary model);
        bool IsExist(string vocabularyName);
        void AddWord(int wordId, int vocabularyId);
        Vocabulary GetByName(string name);
    }
}