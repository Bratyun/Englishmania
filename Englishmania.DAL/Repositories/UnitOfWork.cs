using Englishmania.DAL.EF;
using Englishmania.DAL.Entities;
using Englishmania.DAL.Interfaces;

namespace Englishmania.DAL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly EnglishmaniaContext _dbContext;

        public UnitOfWork(EnglishmaniaContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IRepository<Level> LevelRepository { get; set; }
        public IRepository<Text> TextRepository { get; set; }
        public IRepository<Topic> TopicRepository { get; set; }
        public IRepository<TopicVocabulary> TopicVocabularyRepository { get; set; }
        public IRepository<User> UserRepository { get; set; }
        public IRepository<UserVocabulary> UserVocabularyRepository { get; set; }
        public IRepository<Vocabulary> VocabularyRepository { get; set; }
        public IRepository<Word> WordRepository { get; set; }
        public IRepository<WordText> WordTextRepository { get; set; }
        public IRepository<WordUser> WordUserRepository { get; set; }
        public IRepository<WordVocabulary> WordVocabularyRepository { get; set; }

        public void Commit()
        {
            _dbContext.SaveChanges();
        }
    }
}
