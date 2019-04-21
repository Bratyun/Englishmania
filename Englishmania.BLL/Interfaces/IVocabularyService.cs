using System.Collections.Generic;
using Englishmania.DAL.Entities;

namespace Englishmania.BLL.Interfaces
{
    public interface IVocabularyService
    {
        List<Vocabulary> GetByUser(int userId);
        double GetProgress(int userId, int vocabularyId);
    }
}