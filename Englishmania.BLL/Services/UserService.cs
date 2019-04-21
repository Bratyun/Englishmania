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
            _unitOfWork.UserRepository.Create(model);
            _unitOfWork.Commit();
        }

        public bool IsExist(string login, string passwordHash)
        {
            return Get(login, passwordHash) == null;
        }

        public User Get(string login, string passwordHash)
        {
            return _unitOfWork.UserRepository.Get(x => x.Login == login && x.PasswordHash == passwordHash);
        }
    }
}