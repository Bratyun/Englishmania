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

        public IRepository<Level> LevelRepository => new GenericRepository<Level>(_dbContext);
        public IRepository<Text> TextRepository => new GenericRepository<Text>(_dbContext);
        public IRepository<Topic> TopicRepository => new GenericRepository<Topic>(_dbContext);
        public IRepository<TopicVocabulary> TopicVocabularyRepository => new GenericRepository<TopicVocabulary>(_dbContext);
        public IRepository<User> UserRepository => new GenericRepository<User>(_dbContext);
        public IRepository<Vocabulary> VocabularyRepository => new GenericRepository<Vocabulary>(_dbContext);
        public IRepository<Word> WordRepository => new GenericRepository<Word>(_dbContext);
        public IRepository<WordText> WordTextRepository => new GenericRepository<WordText>(_dbContext);
        public IRepository<WordUser> WordUserRepository => new GenericRepository<WordUser>(_dbContext);
        public IRepository<WordVocabulary> WordVocabularyRepository => new GenericRepository<WordVocabulary>(_dbContext);
        public void Commit()
        {
            _dbContext.SaveChanges();
        }
    }
}
