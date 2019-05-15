using System;
using System.Collections.Generic;
using System.Text;
using Englishmania.BLL.Dto;

namespace Englishmania.BLL.Interfaces
{
    public interface IGameService
    {
        GameTranslationDto GetTranslationGameFromEngToRus(int userId);
        GameTranslationDto GetTranslationGameFromRusToEng(int userId);
        void SetWordProgress(int userId, GameTranslationResult model);
    }
}
