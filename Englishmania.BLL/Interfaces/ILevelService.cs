using Englishmania.DAL.Entities;

namespace Englishmania.BLL.Interfaces
{
    public interface ILevelService
    {
        Level GetByWord(int userId, int wordId);
    }
}
