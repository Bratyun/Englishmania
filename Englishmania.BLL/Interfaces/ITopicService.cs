using System.Collections.Generic;
using Englishmania.DAL.Entities;

namespace Englishmania.BLL.Interfaces
{
    public interface ITopicService
    {
        List<Topic> GetByVocabulary(int vocabularyId);
    }
}