using Englishmania.BLL.Interfaces;
using Englishmania.DAL.Entities;
using Englishmania.DAL.Interfaces;

namespace Englishmania.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void Create(User model)
        {
            var vocabulary = new Vocabulary
            {
                IsPrivate = true,
                LevelId = 0,
                Name = "Personal: " + model.Login
            };
            _unitOfWork.VocabularyRepository.Create(vocabulary);
            _unitOfWork.Commit();
            var vocabularyWithId = _unitOfWork.VocabularyRepository.Get(x => x.Name == vocabulary.Name);
            if (vocabularyWithId == null) return;
            _unitOfWork.UserRepository.Create(model);
            _unitOfWork.Commit();
            var user = _unitOfWork.UserRepository.Get(x => x.Login == model.Login);
            if (user == null) return;
            var connect = new UserVocabulary
            {
                UserId = user.Id,
                VocabularyId = vocabularyWithId.Id
            };
            _unitOfWork.UserVocabularyRepository.Create(connect);
            _unitOfWork.Commit();
        }

        public bool IsExist(string login, string passwordHash)
        {
            return Get(login, passwordHash) != null;
        }

        public User Get(string login, string passwordHash)
        {
            return _unitOfWork.UserRepository.Get(x => x.Login == login && x.PasswordHash == passwordHash);
        }

        public User Get(int id)
        {
            return _unitOfWork.UserRepository.Get(id);
        }
    }
}