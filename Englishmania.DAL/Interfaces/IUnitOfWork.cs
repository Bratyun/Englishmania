using Englishmania.DAL.Entities;

namespace Englishmania.DAL.Interfaces
{
    public interface IUnitOfWork
    {
        IRepository<Level> LevelRepository { get; }
        IRepository<Text> TextRepository { get; }
        IRepository<Topic> TopicRepository { get; }
        IRepository<TopicVocabulary> TopicVocabularyRepository { get; }
        IRepository<User> UserRepository { get; }
        IRepository<UserVocabulary> UserVocabularyRepository { get; }
        IRepository<Vocabulary> VocabularyRepository { get; }
        IRepository<Word> WordRepository { get; }
        IRepository<WordText> WordTextRepository { get; }
        IRepository<WordUser> WordUserRepository { get; }
        IRepository<WordVocabulary> WordVocabularyRepository { get; }

        void Commit();
    }
}
