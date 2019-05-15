using Englishmania.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Englishmania.DAL.EF
{
    public class EnglishmaniaContext : DbContext
    {
        public EnglishmaniaContext(DbContextOptions<EnglishmaniaContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Level> Levels { get; set; }
        public DbSet<Text> Texts { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<TopicVocabulary> TopicVocabularies { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserVocabulary> UserVocabularies { get; set; }
        public DbSet<Vocabulary> Vocabularies { get; set; }
        public DbSet<Word> Words { get; set; }
        public DbSet<WordText> WordTexts { get; set; }
        public DbSet<WordUser> WordUsers { get; set; }
        public DbSet<WordVocabulary> WordVocabularies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<TopicVocabulary>()
                .HasKey(t => new {t.TopicId, t.VocabularyId});
            modelBuilder.Entity<WordText>()
                .HasKey(t => new {t.WordId, t.TextId});
            modelBuilder.Entity<WordUser>()
                .HasKey(t => new {t.WordId, t.UserId});
            modelBuilder.Entity<WordVocabulary>()
                .HasKey(t => new {t.WordId, t.VocabularyId});
            modelBuilder.Entity<UserVocabulary>()
                .HasKey(t => new {t.UserId, t.VocabularyId});
            modelBuilder.Entity<User>(entity => { entity.HasIndex(t => t.Login).IsUnique(); });
            modelBuilder.Entity<Level>().HasData(
                new Level
                {
                    Id = 1,
                    Name = "Low"
                },
                new Level
                {
                    Id = 2,
                    Name = "Medium"
                },
                new Level
                {
                    Id = 3,
                    Name = "High"
                });
        }
    }
}