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
    }
}