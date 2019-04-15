﻿using System;
using System.Collections.Generic;
using System.Text;
using Englishmania.DAL.Entities;

namespace Englishmania.BLL.Interfaces
{
    public interface IVocabularyService
    {
        IList<Vocabulary> GetByUser(int userId);
        double GetProgress(int userId, int vocabularyId);

    }
}