using System;
using System.Collections.Generic;
using System.Text;
using Englishmania.BLL.DTO;

namespace Englishmania.BLL.Interfaces
{
    public interface IWordService
    {
        IList<WordDto> GetByVocabulary(int userId, int vocabularyId);
        int GetProgress(int userId, int wordId);
    }
}
