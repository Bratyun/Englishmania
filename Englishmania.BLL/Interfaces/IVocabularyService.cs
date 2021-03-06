﻿using System.Collections.Generic;
using Englishmania.DAL.Entities;

namespace Englishmania.BLL.Interfaces
{
    public interface IVocabularyService
    {
        List<Vocabulary> GetAll();
        Vocabulary Get(int id);
        List<Vocabulary> GetByUser(int userId);
        double GetProgress(int userId, int vocabularyId);
        double GetGlobalProgress(int userId);
        void ConnectUserWithDictionary(int userId, int vocabularyId);
        void DeleteDictionaryFromUser(int userId, int vocabularyId);
        void Create(Vocabulary model);
        bool IsExist(string vocabularyName);
        void AddWord(int wordId, int vocabularyId);
        Vocabulary GetByName(string name);
        void AddCustomWord(int userId, Word word);
        void DeleteCustomWord(int userId, int wordId);
    }
}